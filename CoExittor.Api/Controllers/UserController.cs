using CoExittor.Common.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoExittor.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [Authorize]
        [HttpGet("events/{userID}")]
        public IActionResult GetUserEvents([FromRoute] long userID)
        {
            throw new NotImplementedException();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserAuthorizationDTO authorizationDTO)
        {
            throw new NotImplementedException();
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegistrationDTO userRegistrationDTO)
        {
            throw new NotImplementedException();
        }
    }
}
