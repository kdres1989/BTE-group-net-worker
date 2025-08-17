using BTE_group_net.Infrastructure.Interfaces.Core;
using BTE_group_net_worker.Core.Interface.Queries;
using BTE_group_net_worker.Models;
using BTE_group_net_worker.Models.VisualModels;
using SqlKata;
using SqlKata.Compilers;

namespace BTE_group_net_worker.Queries
{
    public class PlanParadasQueries : IPlanParadasQueries
    {
        private readonly IDapper _db;
        private readonly SqlServerCompiler _Compiler;

        public PlanParadasQueries(IDapper db)
        {
            _Compiler = new SqlServerCompiler();
            _db = db;
        }

        public async Task<List<VistaParadas>> PlanParadasByFechaAndTipoDemora(DateTime fecha,  string connectionString, int maquina)
        {
            try
            {
                Query query = _db.AsQuery<PlanParadas>();
                query.Select("Codigo");
                query.SelectRaw("(sum(Duracion)) as Duracion");
                query.Where("Maquina", maquina);
                query.Where("FechaInicio", ">=", fecha.Date.AddHours(-2));
                query.Where("FechaFin", "<=", fecha.Date.AddHours(22));
                query.GroupBy("Codigo");
                //Console.WriteLine(_Compiler.Compile(query));
                var result = await _db.QueryAsync<VistaParadas>(query, connectionString);
                return result.ToList();
            }
            catch
            {
                throw;
            }
        }

    }
}
