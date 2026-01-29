using CoExittor.Common.Models;

namespace CoExittor.Api.Domain.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByEmail(string email, CancellationToken token);
    }
}
