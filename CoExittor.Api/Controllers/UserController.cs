using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using CoExittor.Api.Application.Services.Interfaces;
using CoExittor.Api.Services;
using CoExittor.Common.DTO.User;
using CoExittor.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoExittor.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        [Authorize]
        [HttpGet("events/{userID}")]
        public IActionResult GetUserEvents([FromRoute] long userID, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUser([FromRoute] long id, CancellationToken token)
        {
            User? user = await userService.GetUserByID(id, token);
            if (user is not null)
                return Ok(user);
            return BadRequest();
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] UserAuthorizationDTO authorizationDTO, CancellationToken token)
        {
            User? user = await userService.VerifyUser(authorizationDTO, token);
            if(user is not null)
            {
                await AuthService.IssueCookie(user, HttpContext);
                return Ok(user);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDTO registrationDTO, CancellationToken token)
        {
            User? user = await userService.RegisterUser(registrationDTO, token);
            if(user is not null)
            {
                await AuthService.IssueCookie(user, HttpContext);
                return CreatedAtAction(nameof(GetUser), new { id = user.ID });
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
