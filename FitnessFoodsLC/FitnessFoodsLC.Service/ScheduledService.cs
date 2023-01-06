using Microsoft.Extensions.Hosting;

using NCrontab;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessFoodsLC.Service
{
    public class ScheduledService : IHostedService
    {
        private readonly CrontabSchedule _crontabSchedule;
        private DateTime _nextRun;
        private readonly Task _task ;

        public ScheduledService(string url, string schedule)
        {
            _crontabSchedule = CrontabSchedule.Parse(schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
            _task = InitiateImportation(url);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(UntilNextExecution(), cancellationToken); // wait until next time

                    _task.Start(); //execute some task

                    _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }

        private int UntilNextExecution() => Math.Max(0, (int)_nextRun.Subtract(DateTime.Now).TotalMilliseconds);

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private async Task InitiateImportation(string url)
        {
            HttpClient client = new();
            await client.GetAsync(url+"/products/teste");
        }
    }
}
