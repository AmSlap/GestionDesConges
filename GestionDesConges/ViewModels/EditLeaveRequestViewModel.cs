using System.ComponentModel.DataAnnotations;
using GestionDesConges.Data.enums;

namespace GestionDesConges.ViewModels
{
    public class EditLeaveRequestViewModel
    {
        [Key]
        public int LeaveRequestId { get; set; }

        public int EmployeeId { get; set; }

        public AbsenceType AbsenceType { get; set; }

        public int NumberOfDays { get; set; }

        public DateTime StartDate { get; set; }

        public string Reason { get; set; }

    }
}
