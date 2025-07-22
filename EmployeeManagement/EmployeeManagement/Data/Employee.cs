using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Data
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(25)")]
        public string Name { get; set; }

        public decimal Salary { get; set; }

        [Column(TypeName = "nvarchar(25)")]
        public string Location { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(25)")]
        public string Department { get; set; }

        [Column(TypeName = "nvarchar(25)")]
        public string Qualification { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
