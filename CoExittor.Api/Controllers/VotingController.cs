using CoExittor.Common.DTO.Voting;
using Microsoft.AspNetCore.Mvc;

namespace CoExittor.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VotingController : ControllerBase
    {
        /// <summary>
        /// Замещает голосования участника на новые
        /// </summary>
        [HttpPost("update/{eventCode}")]
        public IActionResult UpdateVoting(
            [FromRoute] string eventCode,
            [FromBody] UpdateVotingDTO votingDTO)
        {
            throw new NotImplementedException();
        }
    }
}
