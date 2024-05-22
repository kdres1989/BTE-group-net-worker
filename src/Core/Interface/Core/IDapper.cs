using BTE_group_net.Core;
using SqlKata;

namespace BTE_group_net.Infrastructure.Interfaces.Core
{
    public interface IDapper : IDisposable
    {
        string ReadDataBaseConnection();
        public Query AsQuery<T>();
        Task<IEnumerable<T>> QueryAsync<T>(Query q, string? ConnectionString = null);
        Task<IEnumerable<T>> QueryAsync<T>(string q, string? ConnectionString = null);
        Task<object> ExecuteScalarAsync(string sql, string? ConnectionString = null);
        Task<T> QueryFirstOrDefaultAsync<T>(Query q, string? ConnectionString = null);
        Task<T> QueryFirstAsync<T>(Query q, string? ConnectionString = null);
        Task<int> InsertAsync<T>(T entityToInsert, string? ConnectionString = null) where T : class;
        Task<bool> UpdateAsync<T>(T entityToUpdate, string? ConnectionString = null) where T : class;
        Task<bool> DeleteAsync<T>(T entityToDelete, string? ConnectionString = null) where T : class;
        Task<BulkStatics> BulkInsertAsync<T>(IEnumerable<T> entity, string? ConnectionString = null);
    }
}
