using EmployeeManagement.Data;

namespace EmployeeManagement.Models
{
    public class EmployeeResponse
    {
        public List<Employee> Employees { get; set; }

        public int TotalCount {  get; set; }

        public int TotalPages {  get; set; }
    }
}
