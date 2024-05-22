using BTE_group_net.Infrastructure.Interfaces.Core;
using BTE_group_net_worker.Core.Interface.Queries;
using BTE_group_net_worker.Models;
using BTE_group_net_worker.Models.VisualModels;
using SqlKata;
using SqlKata.Compilers;
using System.Text;

namespace BTE_group_net_worker.Queries
{
    public class PlanProduccionQueries : IPlanProduccionQueries
    {
        private readonly IDapper _db;
        private readonly SqlServerCompiler _Compiler;

        public PlanProduccionQueries(IDapper db)
        {
            _Compiler = new SqlServerCompiler();
            _db = db;
        }

        public async Task<VistaProduccion> PlanProduccionByFechaAndMaquina(DateTime fecha, string connectionString, int maquina, double TiempoDisponible, double TiempoNeto, double TiempoCalendario)
        {
            try
            {
                Query query = _db.AsQuery<PlanProduccion>();
                StringBuilder produccion = new();
                produccion.Append("IFNULL((");
                produccion.AppendFormat("(({0} * ({1} / {0}))", TiempoCalendario, TiempoDisponible);
                produccion.AppendLine();
                produccion.AppendFormat("* ({0} / {1}))", TiempoNeto, TiempoDisponible);
                produccion.AppendLine();
                produccion.AppendLine("* ((1 / SUM(((planproduccion.Mix * 1) / stdpneta.ToneladasHora))) * 100)), 0) AS Produccion, ");

                produccion.AppendLine("IFNULL((");
                produccion.AppendFormat("(({0} * ({1} / {0}))", TiempoCalendario, TiempoDisponible);
                produccion.AppendLine();
                produccion.AppendFormat("* ({0} / {1}))", TiempoNeto, TiempoDisponible);
                produccion.AppendLine();
                produccion.AppendLine("* ((1 / SUM(((planproduccion.Mix * 1) / stdpneta.ToneladasHoraEstandar))) * 100)), 0) AS ProduccionEstandar, ");

                produccion.AppendLine("IFNULL(((1 / SUM(((planproduccion.Mix * 1) / stdpneta.ToneladasHora))) * 100), 0) AS PNetaStd, ");

                produccion.AppendLine("IFNULL(((1 / SUM(((planproduccion.Mix * 1) / stdpneta.ToneladasHoraEstandar))) * 100), 0) AS PNetaStdEstandar");


                query.SelectRaw(produccion.ToString());
                query.LeftJoin("stdpneta", "stdpneta.Id", "planproduccion.StdPNeta");
                query.Where("Fecha", fecha);
                query.Where("stdpneta.Maquina", maquina);
                query.GroupBy("stdpneta.Maquina");
                //Console.WriteLine(_Compiler.Compile(query));
                var result = await _db.QueryFirstOrDefaultAsync<VistaProduccion>(query, connectionString);
                return result;

            }
            catch 
            {
                throw;
            }

        }

    }
}
