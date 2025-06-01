using AccountingManagement.Application.Abstractions;

namespace AccountingManagement.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}