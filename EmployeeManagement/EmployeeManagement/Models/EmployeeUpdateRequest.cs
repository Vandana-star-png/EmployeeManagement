using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class EmployeeUpdateRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(30)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(30)]
        public string Location { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Department { get; set; }
    }
}
