using BonProfCa.Contexts;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace BonProfCa.Utilities;

public class SlotCleaningJob
{
    private readonly MainContext _context;

    public SlotCleaningJob(MainContext context)
    {
        _context = context;
    }

    public void CleanSlots()
    {
        var res = _context
            .Slots
            .Where(s =>
                s.DateFrom < DateTime.Now
                && (
                    s.Reservations.Any(r => r.Status.Name == nameof(StatusReservationCode.Pending))
                    || s.Reservations.Count == 0
                )
            )
            .ExecuteDelete();
        Console.WriteLine("res  "  + res);
    }
}

public class SchedulerHostedService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        RecurringJob.AddOrUpdate<SlotCleaningJob>(
            "DailyCleanSlots",
            job => job.CleanSlots(),
            Cron.Daily
        );
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
