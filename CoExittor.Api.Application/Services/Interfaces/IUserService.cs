using CoExittor.Common.DTO.User;
using CoExittor.Common.Models;

namespace CoExittor.Api.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<Event>> GetUserEvents(long userID);
        Task<bool> VerifyUser(UserAuthorizationDTO dto);
        Task RegisterUser(UserRegistrationDTO dto);
    }
}
