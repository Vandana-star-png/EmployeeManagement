using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace EmployeeManagement.Repository
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllEmployeesAsync();

        Task<Employee> GetEmployeeByIdAsync(int employeeId);

        Task<Employee> AddEmployeeAsync(EmployeeRequest employeeRequest);

        Task<Employee> UpdateEmployeeAsync(Employee employee, EmployeeRequest employeeRequest);

        Task<Employee> UpdateEmployeePatchAsync(Employee employee, JsonPatchDocument employeeRequest);

        Task<Employee> DeleteEmployeeAsync(Employee employee);

        Task<bool> IsEmployeeExistsAsync(string email);
    }
}