using CoExittor.Common.DTO.User;
using CoExittor.Common.Models;

namespace CoExittor.Api.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<Event>> GetUserEvents(long userID, CancellationToken token);
        Task<User?> GetUserByID(long userID, CancellationToken token);
        Task<User?> VerifyUser(UserAuthorizationDTO dto, CancellationToken token);
        Task<User?> RegisterUser(UserRegistrationDTO dto, CancellationToken token);
    }
}
