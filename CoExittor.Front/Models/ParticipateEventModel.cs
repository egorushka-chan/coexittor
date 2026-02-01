using System.ComponentModel.DataAnnotations;

namespace CoExittor.Front.Models
{
    public class ParticipateEventModel
    {
        public Guid EventCode { get; set; }

        [Display(Name = "Ваше имя")]
        [Required, StringLength(60)]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Я согласен(на) с датами хоста")]
        public bool IsAgreedWithDefault { get; set; }

        [Display(Name = "Варианты дат (диапазоны), если нужно указать свой график")]
        public List<VotingModel> Votings { get; set; } = [];
    }
}
