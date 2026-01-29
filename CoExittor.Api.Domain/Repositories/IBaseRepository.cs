using CoExittor.Common.Models.Interfaces;

namespace CoExittor.Api.Domain.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : IEntity
    {
        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken);
        Task CreateAsync(TEntity entity, CancellationToken cancellationToken);
        Task<bool> RemoveByIdAsync(long id, CancellationToken cancellationToken);
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
