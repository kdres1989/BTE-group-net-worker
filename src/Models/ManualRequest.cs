using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTE_group_net_worker.Models
{
    public class ManualRequest
    {
        public int Empresa { get; set; }
        public List<int> Maquinas { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
