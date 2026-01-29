using System.Text.RegularExpressions;
using CoExittor.Api.Application.Services.Interfaces;
using CoExittor.Api.Domain.Repositories;
using CoExittor.Common.DTO.User;
using CoExittor.Common.Extensions;
using CoExittor.Common.Models;

namespace CoExittor.Api.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User?> GetUserByID(long userID, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<List<Event>> GetUserEvents(long userID, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> RegisterUser(UserRegistrationDTO dto, CancellationToken token)
        {
            string hashPassword = dto.PlainPassword.ToSHA512Hex();
            User newUser = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                HashPassword = hashPassword,
                IsActive = true
            };
            await _userRepository.CreateAsync(newUser, token);
            return newUser;
        }

        public async Task<User?> VerifyUser(UserAuthorizationDTO dto, CancellationToken token)
        {
            User? user = await _userRepository.GetByEmail(dto.Email, token);
            if(user == null)
            {
                return null;
            }

            string hashPassword = dto.PlainPassword.ToSHA512Hex();
            if(user.HashPassword == hashPassword)
            {
                return user;
            }
            else
            {
                return null;
            }
        }
    }
}
