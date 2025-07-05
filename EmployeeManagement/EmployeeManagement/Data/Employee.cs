using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Data
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Qualification { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
