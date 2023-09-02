namespace Sample.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var file = Path.Combine(AppContext.BaseDirectory, "dev.trace");
                var idx = 0;
                while (idx++ >= 0)
                {
                    await Task.Delay(1000);
                    using (var sw = new StreamWriter(file, true))
                    {
                        await sw.WriteLineAsync(idx.ToString());
                    }
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}