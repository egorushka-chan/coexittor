using System.Threading;
using CoExittor.Api.Domain.Repositories;
using CoExittor.Common.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoExittor.Api.Infrastructure.Repositories
{
    internal class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly MainDbContext _context;

        public BaseRepository(MainDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(TEntity entity, CancellationToken token)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync(token);
        }

        public async Task<List<TEntity>> GetAllAsync(CancellationToken token)
        {
            return await _context.Set<TEntity>()
                .AsNoTracking()
                .ToListAsync(token);
        }

        public async Task<TEntity?> GetByIdAsync(long id, CancellationToken token)
        {
            return await _context.Set<TEntity>()
                .Where(x => x.ID == id)
                .AsNoTracking()
                .FirstOrDefaultAsync(token);
        }

        public async Task<bool> RemoveByIdAsync(long id, CancellationToken token)
        {
            int count = await _context.Set<TEntity>()
                .Where(ent => ent.ID == id)
                .ExecuteDeleteAsync(token);
            return count > 0;
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken token)
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync(token);
        }
    }
}
