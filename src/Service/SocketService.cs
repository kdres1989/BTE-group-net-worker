using BTE_group_net_worker.Core.Interface.Bridge;
using BTE_group_net_worker.Models;
using System.Net;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;

namespace BTE_group_net_worker.Service
{
    public class SocketService : BackgroundService
    {
        private readonly ILogger<SocketService> _logger;
        private readonly SocketConfig _config;
        private readonly HttpListener _listener = new();
        private readonly IBridge _bridge;

        public SocketService(ILogger<SocketService> logger, SocketConfig config, IBridge bridge)
        {
            _logger = logger;
            _config = config;
            _bridge = bridge;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                string hostname = (_config.Hostname == "0.0.0.0") ? "*" : _config.Hostname;
                _listener.Prefixes.Add($"http://{hostname}:{_config.Port}/");
                _listener.Start();

                _logger.LogInformation($"Worker Consolidado started listening Host '{_config.Hostname}' on port '{_config.Port}'");

                while (!cancellationToken.IsCancellationRequested)
                {
                    var httpContext = await _listener.GetContextAsync().ConfigureAwait(false);
                    ThreadPool.QueueUserWorkItem(async x => await Process((HttpListenerContext)x, cancellationToken).ConfigureAwait(false), httpContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error iniciando listener personalizado");
            }
        }


        private async Task Process(HttpListenerContext client, CancellationToken cancellationToken)
        {
            var request = client.Request;

            using (var response = client.Response)
            {
                try
                {
                    string requestBody;
                    using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        requestBody = reader.ReadToEnd();
                    }
                    ManualRequest requestManual = JsonConvert.DeserializeObject<ManualRequest>(requestBody);

                    await _bridge.RunManual(requestManual);
                    response.StatusCode = 200;
                }
                catch 
                {
                    response.StatusCode = 500;
                }
                
            }
        }


    }
}
