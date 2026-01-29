namespace CoExittor.Common.DTO.Voting
{
    public class UpdateVotingDTO
    {
        public required long ParticipationID { get; set; }
        public List<VotingDTO> Votings { get; set; } = [];
    }
}
