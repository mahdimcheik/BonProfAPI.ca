
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace BonProfCa.Utilities;
    public class Sort
    {
        public string Field { get; }
        public int Order { get; }
    }

    public class FilterItem
    {
        public required object Value { get; set; }
        public required string MatchMode { get; set; }
        public bool? SpecialFilter { get; set; } = false;
    }

    public class DynamicFilters<T>
    {
        public int First { get; set; }
        public int Rows { get; set; }
        public string? Search { get; set; }
        public string GlobalSearch { get; set; }
        public List<Sort> sorts { get; set; }
        public Dictionary<string, FilterItem> Filters { get; set; } = new();
    }

    public static class FilterExtensions
    {
        public static IQueryable<T> ApplySorts<T>(
            this IQueryable<T> query,
            DynamicFilters<T> dynamicFilters
        )
        {
            // If no sorts specified, apply default sort by first Guid property
            if (dynamicFilters.sorts == null || !dynamicFilters.sorts.Any())
            {
                return ApplyDefaultSort(query);
            }

            // Get valid sorts (Order != 0) and order them by priority
            var validSorts = dynamicFilters.sorts
                .Where(s => s.Order != 0)
                .OrderBy(s => Math.Abs(s.Order))
                .ToList();

            if (!validSorts.Any())
            {
                return ApplyDefaultSort(query);
            }

            // Apply first sort using OrderBy/OrderByDescending
            query = ApplyPrimarySort(query, validSorts.First());

            // Apply remaining sorts using ThenBy/ThenByDescending
            foreach (var sort in validSorts.Skip(1))
            {
                query = ApplySecondarySort(query, sort);
            }

            return query;
        }

        private static IQueryable<T> ApplyDefaultSort<T>(IQueryable<T> query)
        {
            var properties = typeof(T).GetProperties();
            var guidProperty = properties.FirstOrDefault(x => x.PropertyType == typeof(Guid));
            
            if (guidProperty == null)
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.Property(parameter, guidProperty);
            var lambda = Expression.Lambda(propertyAccess, parameter);

            return ApplySortMethod(query, lambda, "OrderBy", guidProperty.PropertyType);
        }

        private static IQueryable<T> ApplyPrimarySort<T>(IQueryable<T> query, Sort sort)
        {
            var properties = typeof(T).GetProperties();
            var property = GetProperty(sort.Field, properties);
            
            if (property == null)
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.Property(parameter, property);
            var lambda = Expression.Lambda(propertyAccess, parameter);

            var methodName = sort.Order > 0 ? "OrderBy" : "OrderByDescending";
            return ApplySortMethod(query, lambda, methodName, property.PropertyType);
        }

        private static IQueryable<T> ApplySecondarySort<T>(IQueryable<T> query, Sort sort)
        {
            var properties = typeof(T).GetProperties();
            var property = GetProperty(sort.Field, properties);
            
            if (property == null)
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.Property(parameter, property);
            var lambda = Expression.Lambda(propertyAccess, parameter);

            var methodName = sort.Order > 0 ? "ThenBy" : "ThenByDescending";
            return ApplySortMethod(query, lambda, methodName, property.PropertyType);
        }

        private static IQueryable<T> ApplySortMethod<T>(
            IQueryable<T> query, 
            LambdaExpression lambda, 
            string methodName, 
            Type propertyType)
        {
            var method = typeof(Queryable)
                .GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), propertyType);

            return (IQueryable<T>)method.Invoke(null, new object[] { query, lambda })!;
        }

        public static IQueryable<T> ApplyDynamicWhere<T>(
            this IQueryable<T> query,
            DynamicFilters<T> dynamicFilters
        )
        {
            var entityType = typeof(T);
            var parameter = Expression.Parameter(entityType, "x");
            var filterExpressions = new List<Expression>();

            foreach (var (key, filter) in dynamicFilters.Filters)
            {
                // Skip invalid or special filters
                if (ShouldSkipFilter(key, filter))
                    continue;

                var filterExpression = BuildFilterExpression(
                    parameter, 
                    entityType, 
                    key, 
                    filter
                );

                if (filterExpression != null)
                {
                    filterExpressions.Add(filterExpression);
                }
            }

            // If no valid filters, return original query
            if (!filterExpressions.Any())
                return query;

            // Combine all filter expressions with AND logic
            var combinedExpression = filterExpressions
                .Aggregate((left, right) => Expression.AndAlso(left, right));

            var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
            return query.Where(lambda);
        }

        private static bool ShouldSkipFilter(string key, FilterItem filter)
        {
            return filter.Value == null 
                || key == "custom" 
                || filter.SpecialFilter == true;
        }

        private static Expression? BuildFilterExpression(
            Expression parameter,
            Type entityType,
            string key,
            FilterItem filter)
        {
            var propertyPath = key.Split('/');
            var collectionInfo = AnalyzePropertyPath(entityType, propertyPath);

            if (collectionInfo.HasCollection)
            {
                return BuildCollectionFilter(
                    parameter, 
                    propertyPath, 
                    filter, 
                    entityType,
                    key,
                    collectionInfo
                );
            }
            else
            {
                return BuildSimplePropertyFilter(
                    parameter, 
                    propertyPath, 
                    filter, 
                    entityType, 
                    key
                );
            }
        }

        private static Expression? BuildSimplePropertyFilter(
            Expression parameter,
            string[] propertyPath,
            FilterItem filter,
            Type entityType,
            string key)
        {
            var (member, propertyType) = GetNestedPropertyExpression(parameter, propertyPath, entityType);
            
            if (member == null || propertyType == null)
            {
                Console.WriteLine($"Property path '{key}' not found on type {entityType.Name}");
                return null;
            }

            return filter.MatchMode.ToLower() == "in"
                ? BuildAnyExpression(filter, member, propertyType, key)
                : BuildComparisonExpression(filter, member, propertyType, key);
        }

        private static Expression? BuildCollectionFilter(
            Expression parameter,
            string[] propertyPath,
            FilterItem filter,
            Type entityType,
            string key,
            CollectionPathInfo collectionInfo)
        {
            return BuildCollectionFilterExpression(
                parameter, 
                propertyPath, 
                filter, 
                entityType,
                key,
                collectionInfo
            );
        }

        /// <summary>
        /// Analyzes a property path to detect collections and their positions
        /// Example: "Teacher/Languages/Id" => CollectionIndex = 1 (Languages is collection)
        /// </summary>
        private static CollectionPathInfo AnalyzePropertyPath(Type entityType, string[] propertyPath)
        {
            Type currentType = entityType;
            
            for (int i = 0; i < propertyPath.Length; i++)
            {
                var property = currentType.GetProperty(
                    propertyPath[i],
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
                );

                if (property == null)
                {
                    return new CollectionPathInfo { HasCollection = false };
                }

                if (IsCollectionType(property.PropertyType))
                {
                    // Found a collection
                    return new CollectionPathInfo 
                    { 
                        HasCollection = true,
                        CollectionIndex = i,
                        CollectionProperty = property
                    };
                }

                currentType = property.PropertyType;
            }

            return new CollectionPathInfo { HasCollection = false };
        }

        /// <summary>
        /// Holds information about collection in a property path
        /// </summary>
        private class CollectionPathInfo
        {
            public bool HasCollection { get; set; }
            public int CollectionIndex { get; set; }
            public PropertyInfo? CollectionProperty { get; set; }
        }

        /// <summary>
        /// Check if a type is a collection type (IEnumerable, ICollection, List, etc.)
        /// </summary>
        private static bool IsCollectionType(Type type)
        {
            if (type == typeof(string))
                return false;

            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        /// <summary>
        /// Build filter expression for collection properties (supports multi-level nesting)
        /// Example: "Teacher/Languages/Id" => x.Teacher.Languages.Any(l => filterValues.Contains(l.Id))
        /// Example: "Languages/Id" => x.Languages.Any(l => filterValues.Contains(l.Id))
        /// </summary>
        private static Expression? BuildCollectionFilterExpression(
            Expression parameter,
            string[] propertyPath,
            FilterItem filter,
            Type entityType,
            string key,
            CollectionPathInfo collectionInfo)
        {
            try
            {
                // Step 1: Navigate to the collection property
                var (collectionExpression, collectionProperty, elementType) = 
                    NavigateToCollection(parameter, entityType, propertyPath, collectionInfo);

                if (collectionExpression == null || collectionProperty == null || elementType == null)
                    return null;

                // Step 2: Build the property access inside the collection (e.g., item.Id)
                var (itemPropertyAccess, itemPropertyType) = 
                    BuildItemPropertyAccess(elementType, propertyPath, collectionInfo);

                if (itemPropertyAccess == null || itemPropertyType == null)
                    return null;

                // Step 3: Build the predicate (e.g., filterValues.Contains(item.Id))
                var predicate = BuildCollectionPredicate(
                    filter, 
                    itemPropertyAccess, 
                    itemPropertyType, 
                    key
                );

                if (predicate == null)
                    return null;

                // Step 4: Wrap in Any() expression
                return BuildAnyExpression(
                    collectionExpression, 
                    elementType, 
                    predicate, 
                    itemPropertyAccess.Parameters[0]
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error building collection filter for '{key}': {ex.Message}");
                return null;
            }
        }

        private static (Expression? collection, PropertyInfo? property, Type? elementType) NavigateToCollection(
            Expression parameter,
            Type entityType,
            string[] propertyPath,
            CollectionPathInfo collectionInfo)
        {
            Expression currentExpression = parameter;
            Type currentType = entityType;

            // Navigate to the collection property (e.g., x.Teacher)
            for (int i = 0; i < collectionInfo.CollectionIndex; i++)
            {
                var property = currentType.GetProperty(
                    propertyPath[i],
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
                );

                if (property == null)
                {
                    Console.WriteLine($"Property '{propertyPath[i]}' not found on type {currentType.Name}");
                    return (null, null, null);
                }

                currentExpression = Expression.Property(currentExpression, property);
                currentType = property.PropertyType;
            }

            // Get the collection property itself (e.g., Languages)
            var collectionProperty = currentType.GetProperty(
                propertyPath[collectionInfo.CollectionIndex],
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
            );

            if (collectionProperty == null)
            {
                Console.WriteLine($"Collection property '{propertyPath[collectionInfo.CollectionIndex]}' not found");
                return (null, null, null);
            }

            // Determine element type
            var collectionType = collectionProperty.PropertyType;
            if (!collectionType.IsGenericType)
            {
                Console.WriteLine($"Cannot determine element type for collection '{propertyPath[collectionInfo.CollectionIndex]}'");
                return (null, null, null);
            }

            var elementType = collectionType.GetGenericArguments()[0];
            var collectionAccess = Expression.Property(currentExpression, collectionProperty);

            return (collectionAccess, collectionProperty, elementType);
        }

        private static (LambdaExpression? itemAccess, Type? propertyType) BuildItemPropertyAccess(
            Type elementType,
            string[] propertyPath,
            CollectionPathInfo collectionInfo)
        {
            var lambdaParameter = Expression.Parameter(elementType, "item");
            Expression currentExpression = lambdaParameter;
            Type currentType = elementType;

            // Navigate properties after the collection (e.g., item.Id)
            for (int i = collectionInfo.CollectionIndex + 1; i < propertyPath.Length; i++)
            {
                var property = currentType.GetProperty(
                    propertyPath[i],
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
                );

                if (property == null)
                {
                    Console.WriteLine($"Property '{propertyPath[i]}' not found on type {currentType.Name}");
                    return (null, null);
                }

                currentExpression = Expression.Property(currentExpression, property);
                currentType = property.PropertyType;
            }

            var lambda = Expression.Lambda(currentExpression, lambdaParameter);
            return (lambda, currentType);
        }

        private static Expression? BuildCollectionPredicate(
            FilterItem filter,
            LambdaExpression itemPropertyAccess,
            Type propertyType,
            string key)
        {
            var itemExpression = itemPropertyAccess.Body;

            if (filter.MatchMode.ToLower() == "in")
            {
                return BuildInClausePredicate(filter, itemExpression, propertyType, key);
            }
            else
            {
                return BuildComparisonExpression(filter, itemExpression, propertyType, key);
            }
        }

        private static Expression? BuildInClausePredicate(
            FilterItem filter,
            Expression itemExpression,
            Type propertyType,
            string key)
        {
            var values = ParseFilterValues(filter);
            
            if (values.Length == 0)
                return null;

            var convertedValues = values
                .Select(v => Convert.ChangeType(v, propertyType))
                .ToList();

            var typedArray = Array.CreateInstance(propertyType, convertedValues.Count);
            for (int i = 0; i < convertedValues.Count; i++)
            {
                typedArray.SetValue(convertedValues[i], i);
            }

            var containsMethod = typeof(Enumerable)
                .GetMethods()
                .First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                .MakeGenericMethod(propertyType);

            var arrayConstant = Expression.Constant(typedArray);
            return Expression.Call(null, containsMethod, arrayConstant, itemExpression);
        }

        private static Guid[] ParseFilterValues(FilterItem filter)
        {
            if (filter.Value is JsonElement jsonElement)
            {
                return jsonElement.Deserialize<Guid[]>() ?? Array.Empty<Guid>();
            }
            else if (filter.Value is string strValue)
            {
                return JsonSerializer.Deserialize<Guid[]>(strValue) ?? Array.Empty<Guid>();
            }
            else
            {
                return (Guid[])filter.Value;
            }
        }

        private static Expression BuildAnyExpression(
            Expression collectionExpression,
            Type elementType,
            Expression predicate,
            ParameterExpression lambdaParameter)
        {
            var predicateLambda = Expression.Lambda(predicate, lambdaParameter);

            var anyMethod = typeof(Enumerable)
                .GetMethods()
                .First(m => m.Name == "Any" && m.GetParameters().Length == 2)
                .MakeGenericMethod(elementType);

            return Expression.Call(null, anyMethod, collectionExpression, predicateLambda);
        }

        /// <summary>
        /// Construit une expression pour accéder à une propriété imbriquée
        /// Exemple: "Teacher/FirstName" => x.Teacher.FirstName
        /// </summary>
        private static (Expression? memberExpression, Type? propertyType) GetNestedPropertyExpression(
            Expression parameter,
            string[] propertyPath,
            Type entityType)
        {
            Expression currentExpression = parameter;
            Type currentType = entityType;

            foreach (var propertyName in propertyPath)
            {
                var property = currentType.GetProperty(
                    propertyName,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
                );

                if (property == null)
                {
                    return (null, null);
                }

                currentExpression = Expression.Property(currentExpression, property);
                currentType = property.PropertyType;
            }

            return (currentExpression, currentType);
        }

        /// <summary>
        /// Construit une expression pour le matchMode "any" (IN clause)
        /// </summary>
        private static Expression? BuildAnyExpression(
            FilterItem filter,
            Expression member,
            Type propertyType,
            string key)
        {
            try
            {
                var property = propertyType.GetProperty(key);

                var values = ParseFilterValues(filter);

                if (values.Length > 0)
                {
                    var convertedValues = values
                        .Select(v => Convert.ChangeType(v, propertyType))
                        .ToList();

                    var typedArray = Array.CreateInstance(propertyType, convertedValues.Count);
                    for (int i = 0; i < convertedValues.Count; i++)
                    {
                        typedArray.SetValue(convertedValues[i], i);
                    }

                    var containsMethod = typeof(Enumerable)
                        .GetMethods()
                        .First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                        .MakeGenericMethod(propertyType);

                    var arrayConstant = Expression.Constant(typedArray);
                    return Expression.Call(null, containsMethod, arrayConstant, member);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing 'in' filter for {key}: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Construit une expression de comparaison (equals, contains, gt, etc.)
        /// </summary>
        private static Expression? BuildComparisonExpression(
            FilterItem filter,
            Expression member,
            Type propertyType,
            string key)
        {
            try
            {
                object convertedValue = filter.Value is JsonElement jsonElement
                    ? GetValueFromJsonElement(jsonElement, propertyType)
                    : Convert.ChangeType(filter.Value, propertyType);

                var constant = Expression.Constant(convertedValue, propertyType);

                return filter.MatchMode.ToLower() switch
                {
                    "equals" => BuildEqualsExpression(member, constant, propertyType),
                    "notequals" => BuildNotEqualsExpression(member, constant, propertyType),
                    "contains" => BuildContainsExpression(member, constant, propertyType),
                    "startswith" => BuildStartsWithExpression(member, constant, propertyType),
                    "endswith" => BuildEndsWithExpression(member, constant, propertyType),
                    "gte" => Expression.GreaterThanOrEqual(member, constant),
                    "lte" => Expression.LessThanOrEqual(member, constant),
                    "gt" => Expression.GreaterThan(member, constant),
                    "lt" => Expression.LessThan(member, constant),
                    "after" => Expression.GreaterThan(member, constant),
                    "before" => Expression.LessThan(member, constant),
                    _ => null,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting filter value for {key}: {ex.Message}");
                return null;
            }
        }

        private static Expression BuildEqualsExpression(Expression member, Expression constant, Type propertyType)
        {
            if (propertyType == typeof(string))
            {
                return Expression.Equal(
                    Expression.Call(member, typeof(string).GetMethod("ToLower", Type.EmptyTypes)!),
                    Expression.Call(constant, typeof(string).GetMethod("ToLower", Type.EmptyTypes)!)
                );
            }
            return Expression.Equal(member, constant);
        }

        private static Expression BuildNotEqualsExpression(Expression member, Expression constant, Type propertyType)
        {
            if (propertyType == typeof(string))
            {
                return Expression.NotEqual(
                    Expression.Call(member, typeof(string).GetMethod("ToLower", Type.EmptyTypes)!),
                    Expression.Call(constant, typeof(string).GetMethod("ToLower", Type.EmptyTypes)!)
                );
            }
            return Expression.NotEqual(member, constant);
        }

        private static Expression BuildContainsExpression(Expression member, Expression constant, Type propertyType)
        {
            if (propertyType == typeof(string))
            {
                return Expression.Call(
                    Expression.Call(member, typeof(string).GetMethod("ToLower", Type.EmptyTypes)!),
                    nameof(string.Contains),
                    Type.EmptyTypes,
                    Expression.Call(constant, typeof(string).GetMethod("ToLower", Type.EmptyTypes)!)
                );
            }
            return Expression.Constant(false); // Contains only works on strings
        }

        private static Expression BuildStartsWithExpression(Expression member, Expression constant, Type propertyType)
        {
            if (propertyType == typeof(string))
            {
                return Expression.Call(
                    Expression.Call(member, typeof(string).GetMethod("ToLower", Type.EmptyTypes)!),
                    nameof(string.StartsWith),
                    Type.EmptyTypes,
                    Expression.Call(constant, typeof(string).GetMethod("ToLower", Type.EmptyTypes)!)
                );
            }
            return Expression.Constant(false);
        }

        private static Expression BuildEndsWithExpression(Expression member, Expression constant, Type propertyType)
        {
            if (propertyType == typeof(string))
            {
                return Expression.Call(
                    Expression.Call(member, typeof(string).GetMethod("ToLower", Type.EmptyTypes)!),
                    nameof(string.EndsWith),
                    Type.EmptyTypes,
                    Expression.Call(constant, typeof(string).GetMethod("ToLower", Type.EmptyTypes)!)
                );
            }
            return Expression.Constant(false);
        }

        /// <summary>
        /// Extrait la valeur d'un JsonElement et la convertit au type approprié
        /// </summary>
        private static object GetValueFromJsonElement(JsonElement element, Type targetType)
        {
            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            return underlyingType switch
            {
                Type t when t == typeof(string) => element.GetString() ?? string.Empty,
                Type t when t == typeof(int) => element.GetInt32(),
                Type t when t == typeof(long) => element.GetInt64(),
                Type t when t == typeof(short) => element.GetInt16(),
                Type t when t == typeof(byte) => element.GetByte(),
                Type t when t == typeof(bool) => element.GetBoolean(),
                Type t when t == typeof(decimal) => element.GetDecimal(),
                Type t when t == typeof(double) => element.GetDouble(),
                Type t when t == typeof(float) => element.GetSingle(),
                Type t when t == typeof(Guid) => element.GetGuid(),
                Type t when t == typeof(DateTime) => element.GetDateTime(),
                Type t when t == typeof(DateTimeOffset) => element.GetDateTimeOffset(),
                _ => element.Deserialize(targetType) ?? throw new InvalidOperationException($"Cannot convert JsonElement to {targetType.Name}")
            };
        }

        public static IQueryable<T> ApplyPagination<T>(
            this IQueryable<T> query,
            DynamicFilters<T> dynamicFilters
        )
        {
            if (dynamicFilters.First >= 0)
                query = query.Skip(dynamicFilters.First);

            query = query.Take(dynamicFilters.Rows > 0 ? dynamicFilters.Rows : 10);
            return query;
        }

        public static async Task<(List<T> Values, long Count)> ApplyAndCountAsync<T>(
            this IQueryable<T> query,
            DynamicFilters<T> dynamicFilters
        )
        {
            query = query.ApplyDynamicWhere(dynamicFilters);
            query = query.ApplySorts(dynamicFilters);

            var count = await query.CountAsync();

            query = query.ApplyPagination(dynamicFilters);

            var values = await query.ToListAsync();
            return (values, count);
        }

        public static PropertyInfo? GetProperty(string propName, PropertyInfo[] properties)
        {
            return properties.FirstOrDefault(x => x.Name.ToLower() == propName.ToLower());
        }
    }

