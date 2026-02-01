using CoExittor.Common.DTO.Voting;

namespace CoExittor.Common.DTO.Event
{
    public class ParticipateEventDTO
    {
        public required string Name { get; set; } = string.Empty;
        public bool IsAgreedWithDefault { get; set; }
        public long? LinkedUserID { get; set; }
        public List<VotingDTO> Votings { get; set; } = [];
    }
}
