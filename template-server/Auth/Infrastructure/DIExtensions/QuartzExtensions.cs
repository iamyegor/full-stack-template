using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DIExtensions;

public static class QuartzExtensions
{
    public static IServiceCollection AddOutbox(this IServiceCollection services)
    {
        // services.AddQuartz(q =>
        // {
        //     JobKey jobKey = JobKey.Create(nameof(ProcessOutboxMessagesJob));
        //
        //     q.AddJob<ProcessOutboxMessagesJob>(jobKey)
        //         .AddTrigger(trigger =>
        //             trigger
        //                 .ForJob(jobKey)
        //                 .WithSimpleSchedule(schedule =>
        //                     schedule.WithIntervalInSeconds(1).RepeatForever()
        //                 )
        //         );
        // });
        //
        // services.AddQuartzHostedService();
        // services.AddScoped<OutboxService>();

        return services;
    }
}
