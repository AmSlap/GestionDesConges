using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GestionDesConges.Data.enums;

namespace GestionDesConges.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        public Position Position { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // Foreign key to Department entity
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public ICollection<LeaveRequest> LeaveRequests { get; set; }
    }
}
