namespace CoExittor.Common.DTO.User
{
    public class UserAuthorizationDTO
    {
        public required string Login { get; set; }
        public required string PlainPassword { get; set; }
    }
}
