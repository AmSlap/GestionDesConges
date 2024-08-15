using System.ComponentModel.DataAnnotations;
using GestionDesConges.Data.enums;

namespace GestionDesConges.ViewModels
{
    public class CreateLeaveRequestViewModel
    {
        public int EmployeeId { get; set; }
        [Required]
        public AbsenceType AbsenceType { get; set; }
        [Required]

        public DateTime StartDate { get; set; }
        [Required]

        public DateTime EndDate { get; set; }
        [Required]

        public string Reason { get; set; }
        [Required]

        public string Comments { get; set; }
    }
}
