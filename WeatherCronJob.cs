using Cronos; // Do to being requested to have this run on Cronjobs,
              // I found it easier to import Cronos that deals with Cronjob times.

namespace FoodDeliveryBackend
{
    /// <summary>
    /// Create a weather Cronjob with the help of this class.
    /// </summary>
    public class WeatherCronJob : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly string _cronSchedule = "0 0 * * *";
        private readonly CronExpression _cronExpression;
        private DateTime _nextRunTime;

        /// <summary>
        /// Setup WeatherCronJob with a scope factory and cron schedule string.
        /// </summary>
        public WeatherCronJob(IServiceScopeFactory scopeFactory, string cronString)
        {
            cronString ??= "0 0 * * *";

            _scopeFactory = scopeFactory;
            _cronSchedule = cronString;

            Console.WriteLine("Setting weather fetcher cron schedule to " + _cronSchedule);
            _cronExpression = CronExpression.Parse(_cronSchedule, CronFormat.Standard);
            // Should be replaced with something that listens to the program for a database connection event or something.
            _nextRunTime = DateTime.UtcNow.AddSeconds(5); // This will make it so that we parse the weather data 5 seconds after booting up.
        }

        /// <summary>
        /// A never ending task that check every 5 seconds if it can run the task of fetching the weather again.
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;
                if (now >= _nextRunTime)
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var weatherFetcher = scope.ServiceProvider.GetRequiredService<WeatherDataFetcher>();
                        await weatherFetcher.FetchAndStoreWeatherDataAsync();
                    }

                    _nextRunTime = _cronExpression.GetNextOccurrence(DateTime.UtcNow, TimeZoneInfo.Utc) ?? DateTime.UtcNow.AddMinutes(1);
                }

                // I do admit that this also can be improved.
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
