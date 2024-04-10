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
    public class TiempoQueries : ITiempoQueries
    {
        private readonly IDapper _db;
        private readonly SqlServerCompiler _Compiler;

        public TiempoQueries(IDapper db)
        {
            _Compiler = new SqlServerCompiler();
            _db = db;
        }

        public async Task<List<VistaParadas>> TiemposByFechaAndTipoDemora(DateTime fecha, int tipoDemora, string connectionString, int maquina)
        {
            try
            {
                Query query = _db.AsQuery<Tiempo>();
                query.Select("Codigo");
                query.SelectRaw("(sum(timestampdiff(MINUTE,`FechaInicio`,`FechaFin`))/60) as Duracion");
                query.LeftJoin("codiguera", "tiempo.Codigo", "codiguera.IdCodigo");
                query.Where("Maquina", maquina);
                query.Where("codiguera.TipoDemora", tipoDemora);
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
