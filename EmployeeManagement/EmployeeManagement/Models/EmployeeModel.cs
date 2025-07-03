using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class EmployeeModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, ErrorMessage = "Name can't exceed 20 characters")]
        public string Name { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Salary must be non-negative value")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(20, ErrorMessage = "Location can't exceed 20 characters")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Please add Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [StringLength(20, ErrorMessage = "Department can't exceed 20 characters")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Qualification is required")]
        [StringLength(20, ErrorMessage = "Qualification can't exceed 20 characters")]
        public string Qualification { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
