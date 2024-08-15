using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GestionDesConges.Data.enums;

namespace GestionDesConges.Models
{
    public class Approval
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApprovalId { get; set; }

        public int LeaveRequestId { get; set; }

        [ForeignKey("LeaveRequestId")]
        public LeaveRequest LeaveRequest { get; set; }

        public int ApproverId { get; set; }

        [ForeignKey("ApproverId")]
        public Employee Approver { get; set; }

        [Required]
        public ApprovalStatus ApprovalStatus { get; set; }

        public string Comments { get; set; }

        public DateTime ApprovalDate { get; set; }
    }
}
