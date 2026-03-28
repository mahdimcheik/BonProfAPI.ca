using BonProfCa.Contexts;
using BonProfCa.Controllers;
using BonProfCa.Models;
using BonProfCa.Services;
using BonProfCa.Services;
using BonProfCa.Services.Interfaces;
using BonProfCa.Utilities;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configurer les services
var services = builder.Services;
ConfigureServices(services);

var app = builder.Build();

app.UseForwardedHeaders(
    new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
    }
);

// Apply database migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MainContext>();
    context.Database.Migrate();

    // Seed default users
    SeedUsers(scope.ServiceProvider);
}

// Configurer le pipeline de middleware
ConfigureMiddlewarePipeline(app);

app.Run();

#region Services
static void ConfigureServices(IServiceCollection services)
{
    // add services
    services.AddTransient<AuthService>();
    services.AddTransient<MailService>();
    services.AddTransient<MinioService>();
    services.AddTransient<AddressesService>();
    services.AddTransient<TypeAddressService>();
    services.AddTransient<FormationsService>();
    services.AddTransient<TeacherService>();
    services.AddTransient<StudentService>();
    services.AddTransient<LanguagesService>();
    services.AddTransient<CursusService>();
    services.AddTransient<LevelCursusService>();
    services.AddTransient<CategoryCursusService>();
    services.AddTransient<ProductService>();
    services.AddTransient<TypeSlotsService>();
    services.AddTransient<SlotsService>();
    services.AddTransient<GendersService>();
    services.AddTransient<RolesService>();
    services.AddTransient<StatusReservationService>();
    services.AddTransient<NotificationService>();
    services.AddTransient<ConversationService>();
    services.AddTransient<SlotCleaningJob>();
    services.AddHostedService<SchedulerHostedService>();

    // signalR
    services.AddSingleton<ConnectionManager>();
    services.AddSignalR(options =>
    {
        options.EnableDetailedErrors = true; // For debugging
    });
    services.AddTransient<SignalRNotificationsService>();

    services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddConsole();
        loggingBuilder.AddDebug();
    });

    services.AddRouting(opt => opt.LowercaseUrls = true);

    var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

    // db
    var connString =
        $"Host={EnvironmentVariables.DB_HOST};"
        + $"Port={EnvironmentVariables.DB_PORT};"
        + $"Database={EnvironmentVariables.DB_NAME};"
        + $"Username={EnvironmentVariables.DB_USER};"
        + $"Password={EnvironmentVariables.DB_PASSWORD};";

    var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
    dataSourceBuilder.EnableDynamicJson();
    var dataSource = dataSourceBuilder.Build();

    services.AddDbContext<MainContext>(options =>
    {
        options.UseNpgsql(dataSource);
    });

    services.Configure<DataProtectionTokenProviderOptions>(options =>
    {
        options.TokenLifespan = TimeSpan.FromHours(1);
    });

    ConfigureCors(services);
    ConfigureControllers(services);
    ConfigureSwagger(services);
    ConfigureIdentity(services);
    ConfigureAuthentication(services);
    ConfigureHangFire(services);
}

static void ConfigureCors(IServiceCollection services)
{
    services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder
                .SetIsOriginAllowed(CorsHelper.IsOriginAllowed)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders("filename");
        });
    });
}

static void ConfigureControllers(IServiceCollection services)
{
    services
        .AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // pour que les enums soient en string dans les JSON, sinon le batard ng-openapi les met en int
        }) ;
    ;
    services.AddEndpointsApiExplorer();
}

static void ConfigureIdentity(IServiceCollection services)
{
    services
        .AddIdentity<UserApp, RoleApp>()
        .AddEntityFrameworkStores<MainContext>()
        .AddRoleManager<RoleManager<RoleApp>>()
        .AddUserManager<UserManager<UserApp>>()
        .AddSignInManager<SignInManager<UserApp>>()
        .AddDefaultTokenProviders();

    services.Configure<IdentityOptions>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 8;
        options.Password.RequiredUniqueChars = 1;

        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        options.User.AllowedUserNameCharacters =
            " abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true;

        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = true;
        options.SignIn.RequireConfirmedPhoneNumber = false;
    });
}

static void ConfigureAuthentication(IServiceCollection services)
{
    services
        .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = EnvironmentVariables.API_BACK_URL,
                ValidAudience = EnvironmentVariables.API_BACK_URL,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(EnvironmentVariables.JWT_KEY)
                ),
            };
        });
}

static void ConfigureHangFire(IServiceCollection services)
{
    // Hangfire configuration can be added here if needed in the future
    services.AddHangfire(configuration =>
        configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(
                (options) =>
                {
                    options.UseNpgsqlConnection(
                        $"Host={EnvironmentVariables.DB_HOST};Port={EnvironmentVariables.DB_PORT};Database={EnvironmentVariables.DB_NAME};Username={EnvironmentVariables.DB_USER};Password={EnvironmentVariables.DB_PASSWORD};CommandTimeout=60;"
                    );
                }
            )
    );

    services.AddHangfireServer();
}

static void ConfigureSwagger(IServiceCollection services)
{
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc(
            "v1",
            new OpenApiInfo
            {
                Title = "BonProf API",
                Version = "v1",
                Description = "API for BonProf application",
            }
        );

        var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFilename);
        if (System.IO.File.Exists(xmlPath))
        {
            c.IncludeXmlComments(xmlPath);
        }

        c.AddSecurityDefinition(
            "Bearer",
            new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description =
                    "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
            }
        );

        c.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                    },
                    new string[] { }
                },
            }
        );
    });

    services.AddHttpClient();
}

#endregion

#region MiddleWare
static void ConfigureMiddlewarePipeline(WebApplication app)
{
    var supportedCultures = new string[] { "fr-FR" };
    app.UseRequestLocalization(options =>
        options
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures)
            .SetDefaultCulture("fr-FR")
    );

    // Add payload size limit middleware early in the pipeline
    app.Use(
        async (context, next) =>
        {
            if (context.Request.ContentLength > 200_000_000)
            {
                context.Response.StatusCode = StatusCodes.Status413PayloadTooLarge;
                await context.Response.WriteAsync("Payload Too Large");
                return;
            }

            await next.Invoke();
        }
    );

    app.UseStaticFiles();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BonProfApi API v1");
        c.RoutePrefix = string.Empty;
        c.DocumentTitle = "BonProfApi API Documentation";
    });

    app.UseHangfireDashboard("/hangfire");

    app.UseRouting();

    app.UseCors();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapHub<ChatHub>("/chathub");
}
#endregion

#region seed data
static void SeedUsers(IServiceProvider serviceProvider)
{
    using (var scope = serviceProvider.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<MainContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserApp>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleApp>>();

        // Seed a default super admin user
        var superAdminEmail = new UserApp
        {
            UserName = EnvironmentVariables.SUPER_ADMIN_EMAIL,
            Email = EnvironmentVariables.SUPER_ADMIN_EMAIL,
            EmailConfirmed = true,
            StatusId = HardCode.ACCOUNT_ACTIVE,
            GenderId = HardCode.GENDER_OTHER,
            FirstName = "Super",
            LastName = "Admin",
            DateOfBirth = DateTimeOffset.Now.AddYears((-20)),
        };
        var superAdminPassword = EnvironmentVariables.SUPER_ADMIN_PASSWORD;
        if (userManager.FindByEmailAsync(superAdminEmail.Email).Result == null)
        {
            var createPowerUser = userManager
                .CreateAsync(superAdminEmail, superAdminPassword)
                .Result;
            if (createPowerUser.Succeeded)
            {
                if (!roleManager.RoleExistsAsync("SuperAdmin").Result)
                {
                    var role = new RoleApp { Name = "SuperAdmin" };
                    roleManager.CreateAsync(role).Wait();
                }
                userManager.AddToRoleAsync(superAdminEmail, "SuperAdmin").Wait();
            }
        }
    }
}
#endregion

