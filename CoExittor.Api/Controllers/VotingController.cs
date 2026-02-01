using System.Threading.Tasks;
using CoExittor.Api.Application.Services.Interfaces;
using CoExittor.Common.DTO.Voting;
using Microsoft.AspNetCore.Mvc;

namespace CoExittor.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VotingController : ControllerBase
    {
        private readonly IVotingService _votingService;

        public VotingController(IVotingService votingService)
        {
            _votingService = votingService;
        }

        /// <summary>
        /// Замещает голосования участника на новые
        /// </summary>
        [HttpPost("update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateVoting(
            [FromBody] UpdateVotingDTO votingDTO,
            CancellationToken token)
        {
            await _votingService.UpdateVotingAsync(votingDTO, token);
            return NoContent();
        }
    }
}
