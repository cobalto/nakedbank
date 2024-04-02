using Coravel;
using Microsoft.Extensions.Hosting;
using NakedBank.WebApi.Invocables;

namespace NakedBank.WebApi.Extensions
{
    public static class SchedulerHostExtensions
    {
        public static IHost SchedulerStartup(this IHost host)
        {
            host.Services.UseScheduler(scheduler =>
            {
                scheduler
                    .Schedule<BalanceUpdateInvocable>()
                    .DailyAtHour(0);
            });

            return host;
        }
    }
}
