using System.ComponentModel.DataAnnotations;

namespace CoExittor.Front.Models
{
    public class VotingModel
    {
        [Display(Name = "Начало")]
        [Required]
        public DateTime StartDate { get; set; }

        [Display(Name = "Конец")]
        [Required]
        public DateTime EndDate { get; set; }
    }
}
