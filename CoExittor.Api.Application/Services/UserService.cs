using CoExittor.Api.Application.Services.Interfaces;
using CoExittor.Common.DTO.User;
using CoExittor.Common.Models;

namespace CoExittor.Api.Application.Services
{
    public class UserService : IUserService
    {
        public Task<List<Event>> GetUserEvents(long userID)
        {
            throw new NotImplementedException();
        }

        public Task RegisterUser(UserRegistrationDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifyUser(UserAuthorizationDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
