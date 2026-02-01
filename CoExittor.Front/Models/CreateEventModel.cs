using System.ComponentModel.DataAnnotations;

namespace CoExittor.Front.Models
{
    public class CreateEventModel
    {
        [Display(Name = "Название")]
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Описание")]
        [StringLength(400)]
        public string? Description { get; set; }

        [Display(Name = "Имя создателя")]
        [Required, StringLength(60)]
        public string HostName { get; set; } = string.Empty;

        [Display(Name = "Варианты дат (диапазоны)")]
        public List<VotingModel> Votings { get; set; } = [new VotingModel()];
    }
}
