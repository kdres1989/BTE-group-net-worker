using BTE_group_net_worker.Core.Interface.Bridge;

namespace BTE_group_net_worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IBridge _bridge;

        public Worker(ILogger<Worker> logger, IBridge bridge)
        {
            _logger = logger;
            _bridge = bridge;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await _bridge.Run();
                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}