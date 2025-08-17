using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BTE_group_net_worker.Models.VisualModels
{
    public class VistaProduccion
    {
        public double Produccion { get; set; }
        public double ProduccionEstandar { get; set; }
        public double PNetaStd { get; set; }
        public double PNetaStdEstandar { get; set; }
    }
}
