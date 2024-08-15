using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GestionDesConges.Data.enums;

namespace GestionDesConges.Models
{
    public class LeaveRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeaveRequestId { get; set; }

        public int EmployeeId { get; set; } //fk

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [Required]
        public AbsenceType AbsenceType { get; set; }

        [Required]
        public int NumberOfDays { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public string Reason { get; set; }

        public ApprovalStatus Status { get; set; } // Pending, Approved, Rejected, etc.

        public string Comments { get; set; }

        // Additional properties and relationships can be added as needed
    }
}
