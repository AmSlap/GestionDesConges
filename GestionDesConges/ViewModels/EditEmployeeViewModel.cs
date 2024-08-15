using System.ComponentModel.DataAnnotations;
using GestionDesConges.Data.enums;
using GestionDesConges.Models;

namespace GestionDesConges.ViewModels
{
    public class EditEmployeeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Position position{ get; set; }
        [EmailAddress]
        public string Email { get; set; }

        public int DepId { get; set; }
        

    }
}
