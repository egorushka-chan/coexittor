namespace CoExittor.Common.DTO.User
{
    public class UserRegistrationDTO
    {
        public required string Nickname { get; set; }
        public required string Email { get; set; }
        public required string PlainPassword { get; set; }
    }
}
