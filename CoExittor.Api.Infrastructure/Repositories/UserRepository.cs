using CoExittor.Api.Domain.Repositories;
using CoExittor.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace CoExittor.Api.Infrastructure.Repositories
{
    internal class UserRepository(MainDbContext context) : BaseRepository<User>(context), IUserRepository
    {
        public async Task<User?> GetByEmail(string email, CancellationToken token)
        {
            User? user = await _context.Users.Where(u => u.Email == email)
                .AsNoTracking()
                .FirstOrDefaultAsync(token);

            return user;
        }
    }
}
