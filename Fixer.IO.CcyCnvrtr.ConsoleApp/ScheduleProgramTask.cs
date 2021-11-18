using System;
using System.Threading;
using System.Threading.Tasks;
using Fixer.IO.CcyCnvrtr.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fixer.IO.CcyCnvrtr.ConsoleApp
{
    internal class ScheduleProgramTask : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;

        public ScheduleProgramTask(ILogger<ScheduleProgramTask> logger)
        {
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching daily rates service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,TimeSpan.FromHours(24));
           
        }

        private async void DoWork(object state)
        {
            _logger.LogInformation("Fetching daily rates service is starting.");
            var serviceProvider = GetServiceProvider();
            var fixerManager = serviceProvider.GetService<IFixerManager>();

            Console.WriteLine(await fixerManager.GetDailyRateAsync());
            _logger.LogInformation(DateTime.Now.ToLongDateString());
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
              "Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public static IServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();
            //
            // Register the services here
            //
            services.UseFixer(new FixerConfig
            {
                BaseUri = "http://data.fixer.io/api/",
                APIKey = "a4793baae7800001f62bd763956be086"
            });

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}
