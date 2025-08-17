using BTE_group_net_worker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTE_group_net_worker.Core.Interface.Queries
{
    public interface IParametroQueries
    {
        Task<Parametro> GetParametroByNombre(string Nombre);
    }
}
