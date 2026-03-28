using BonProfCa.Models;
using System.Diagnostics.CodeAnalysis;

namespace BonProfCa.Models;

public class ProductDetails
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public CursusDetails? Cursus { get; set; }

    [SetsRequiredMembers]
    public ProductDetails(Product product)
    {
        Id = product.Id;
        Name = product.Name;
        Description = product.Description;
        Price = product.Price;
        Cursus = product.Cursus is not null ? new CursusDetails(product.Cursus) : null;
    }
}

public class ProductCreate
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public Guid CursusId { get; set; }
}

public class ProductUpdate
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public Guid CursusId { get; set; }

    public void UpdateProduct(Product product)
    {
        product.Name = Name;
        product.Description = Description;
        product.Price = Price;
        product.CursusId = CursusId;
        product.UpdatedAt = DateTimeOffset.UtcNow;
    }
}
