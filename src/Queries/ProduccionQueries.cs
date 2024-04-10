using BTE_group_net.Infrastructure.Interfaces.Core;
using BTE_group_net_worker.Core.Interface.Queries;
using BTE_group_net_worker.Models;
using BTE_group_net_worker.Models.VisualModels;
using SqlKata;
using SqlKata.Compilers;

namespace BTE_group_net_worker.Queries
{
    public class ProduccionQueries : IProduccionQueries
    {
        private readonly IDapper _db;
        private readonly SqlServerCompiler _Compiler;

        public ProduccionQueries(IDapper db)
        {
            _Compiler = new SqlServerCompiler();
            _db = db;
        }

        public async Task<List<VistaReales>> ProduccionByFechaAndMaquina(DateTime fecha, string connectionString, int maquina)
        {
            try
            {
                Query query = _db.AsQuery<Produccion>();
                query.Select("Peso", "stdpneta.ToneladasHora", "stdpneta.ToneladasHoraEstandar");
                query.LeftJoin("material", "material.IdMaterial", "produccion.Pedido");
                query.LeftJoin("stdpneta", join => join.On("stdpneta.Material", "material.Id").On("stdpneta.Maquina", "produccion.Maquina"));
                query.Where("produccion.Maquina", maquina);
                query.Where("Fecha", fecha);
                Console.WriteLine(_Compiler.Compile(query));
                var result = await _db.QueryAsync<VistaReales>(query, connectionString);
                return result.ToList();
            }
            catch 
            {
                throw;
            }
        }
    }
}
