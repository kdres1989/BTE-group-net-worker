using BTE_group_net_worker.Models.VisualModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTE_group_net_worker.Core.Interface.Queries
{
    public interface IPlanParadasQueries
    {
        Task<List<VistaParadas>> PlanParadasByFechaAndTipoDemora(DateTime fecha, string connectionString, int maquina);
    }
}
