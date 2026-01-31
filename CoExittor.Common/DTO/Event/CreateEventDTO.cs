using CoExittor.Common.DTO.Voting;

namespace CoExittor.Common.DTO.Event
{
    public class CreateEventDTO
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required HostDTO Host { get; set; }   

        public class HostDTO
        {
            public required string Name { get; set; }
            public long? LinkedUserID { get; set; }
            public List<VotingDTO> Votings { get; set; } = [];
        }
    }
}
