using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using BonProfCa.Models.Interfaces;
using BonProfCa.Utilities;

namespace BonProfCa.Models;

[Table("Addresses")]
public class Address : BaseModel
{
    [Required]
    [MaxLength(250)]
    public required string Street { get; set; }

    [Required]
    [MaxLength(150)]
    public required string City { get; set; }

    [Required]
    [MaxLength(150)]
    public required string Country { get; set; }

    [Required]
    [MaxLength(20)]
    public required string ZipCode { get; set; }
    
    [MaxLength(255)]
    public string? AdditionalInfo { get; set; }
    
    public float? Longitude { get; set; }
    
    public float? Latitude { get; set; }
    
    [Required]
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    public UserApp? User { get; set; }
    
    [Required]
    [ForeignKey(nameof(Type))]
    public Guid TypeId { get; set; } = HardCode.TYPE_ADDRESS_MAIN;
    public TypeAddress? Type { get; set; }
    
    public Address()
    {        
    }

    [SetsRequiredMembers]
    public Address(AddressCreate addressDto)
    {
        Id = Guid.NewGuid();
        Street = addressDto.Street;
        City = addressDto.City;
        Country = addressDto.Country;
        ZipCode = addressDto.ZipCode;
        AdditionalInfo = addressDto.AdditionalInfo;
        Longitude = addressDto.Longitude;
        Latitude = addressDto.Latitude;
        UserId = addressDto.UserId;
        TypeId = HardCode.TYPE_ADDRESS_MAIN;
        CreatedAt = DateTimeOffset.UtcNow;
    }
    
    public void UpdateAddress(AddressUpdate addressDto)
    {
        Street = addressDto.Street;
        City = addressDto.City;
        Country = addressDto.Country;
        ZipCode = addressDto.ZipCode;
        AdditionalInfo = addressDto.AdditionalInfo;
        Longitude = addressDto.Longitude;
        Latitude = addressDto.Latitude;
        UpdatedAt = DateTimeOffset.UtcNow;
        TypeId = addressDto.TypeId;
    }
}
