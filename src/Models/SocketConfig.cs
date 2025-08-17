using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTE_group_net_worker.Models
{
    public class SocketConfig
    {
        private readonly IConfiguration _configuration;

        public SocketConfig(IConfiguration configuration) 
        {
            _configuration = configuration;
            this.Hostname = _configuration["Socket:Hostname"];
            this.Port = int.TryParse(_configuration["Socket:Port"], out int puerto) ? puerto : 80;
        }

        public string Hostname { get; set; }
        public int Port { get; set; }

    }
}
