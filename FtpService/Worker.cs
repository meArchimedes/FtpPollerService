using FtpPoller.Data;
using Microsoft.EntityFrameworkCore;

namespace FtpPollerService
{


    public class Worker : BackgroundService
    {
        private readonly string _connectionString;
        private readonly ILogger<Worker> _logger;
        private readonly PollerFunctions _poller;
        private int count = 1;
        FtpPollerContext _context;
        public Worker(IConfiguration configuration, ILogger<Worker> logger)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("FtpDbString");

            var optionsBuilder = new DbContextOptionsBuilder<FtpPollerContext>().UseNpgsql(_connectionString).Options;

            _context = new FtpPollerContext(optionsBuilder);
            _poller = new PollerFunctions(_context, _logger);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                await _poller.TransferFilesAsync();
                _logger.LogInformation($"Worker ran {count} times");
                TimeSpan delayDuration = TimeSpan.FromMinutes(5);
                await Task.Delay(delayDuration, stoppingToken);
                count++;
            }
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker Started");
            return base.StartAsync(cancellationToken);

        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker Stopping");
            return base.StopAsync(cancellationToken);
        }
    }

}