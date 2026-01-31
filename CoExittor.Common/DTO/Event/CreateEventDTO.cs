namespace CoExittor.Common.DTO.Event
{
    public class CreateEventDTO
    {
        public required string Name { get; set; }
        public required bool HardTimed { get; set; }
        public required DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public required HostDTO Host { get; set; }   

        public class HostDTO
        {
            public required string Name { get; set; }
            public long? LinkedUserID { get; set; }
        }
    }
}
