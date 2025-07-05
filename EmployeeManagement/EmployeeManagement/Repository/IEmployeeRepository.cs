using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace EmployeeManagement.Repository
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllEmployeesAsync();

        Task<Employee> GetEmployeeByIdAsync(int employeeId);

        Task<int> AddEmployeeAsync(EmployeeRequest employeeRequest);

        Task<Employee> UpdateEmployeeAsync(Employee employee, EmployeeRequest employeeRequest);

        Task<bool> UpdateEmployeePatchAsync(Employee employee, JsonPatchDocument employeeRequest);

        Task<bool> DeleteEmployeeAsync(Employee employee);

        Task<bool> IsEmployeeExistsAsync(string email);
    }
}