using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BonProfCa.Utilities;

public class CustomDateTimeConversion : ValueConverter<DateTimeOffset, DateTime>
{
    public CustomDateTimeConversion()
        : base(
        dto => dto.UtcDateTime,
        dt => new DateTimeOffset(dt, TimeSpan.Zero))
    {
    }
}
