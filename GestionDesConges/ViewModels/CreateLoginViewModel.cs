using System.ComponentModel.DataAnnotations;

namespace GestionDesConges.ViewModels
{
    public class CreateLoginViewModel
    {
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string PasswordHash { get; set; }
    }
}
