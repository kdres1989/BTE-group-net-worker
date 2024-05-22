using BTE_group_net.Infrastructure.Interfaces.Core;
using BTE_group_net_worker.Core.Interface.Queries;
using BTE_group_net_worker.Models;
using BTE_group_net_worker.Models.VisualModels;
using SqlKata;
using SqlKata.Compilers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTE_group_net_worker.Queries
{
    public class PlanInterrupcionesQueries : IPlanInterrupcionesQueries
    {
        private readonly IDapper _db;
        private readonly SqlServerCompiler _Compiler;

        public PlanInterrupcionesQueries(IDapper db)
        {
            _Compiler = new SqlServerCompiler();
            _db = db;
        }

        public async Task<List<VistaParadas>> PlanInterrupcionesByFechaAndMaquina(DateTime fecha, string connectionString, int maquina, double TiempoDisponible)
        {
            try
            {
                Query query = _db.AsQuery<PlanInterrupciones>();
                query.Select("stduneta.Codigo");
                query.SelectRaw($"(sum(planinterrupciones.Porcentaje/100)) as Duracion");
                query.LeftJoin("stduneta", "stduneta.Id", "planinterrupciones.StdUNeta");
                query.Where("stduneta.Maquina", maquina);
                query.Where("planinterrupciones.Fecha", fecha.Date);
                query.Where("planinterrupciones.Porcentaje", "<>", 0);
                query.GroupBy("stduneta.Codigo");
                //Console.WriteLine(_Compiler.Compile(query));
                var result = await _db.QueryAsync<VistaParadas>(query, connectionString);
                result = result.Where(r => r.Duracion != 0);
                return result.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
