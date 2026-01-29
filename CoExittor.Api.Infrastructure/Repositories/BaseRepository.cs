using CoExittor.Api.Domain.Repositories;
using CoExittor.Common.Models.Interfaces;

namespace CoExittor.Api.Infrastructure.Repositories
{
    internal class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : IEntity
    {
        public Task CreateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveByIdAsync(long id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
