using System.ComponentModel.DataAnnotations;

namespace CoExittor.Common.DTO.User
{
    public class UserAuthorizationDTO
    {
        [Required(ErrorMessage = "Логин не может быть пустым")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Пароль не может быть пустым")]
        public required string PlainPassword { get; set; }
    }
}
