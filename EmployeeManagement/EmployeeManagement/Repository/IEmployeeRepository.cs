using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace EmployeeManagement.Repository
{
    public interface IEmployeeRepository
    {
        Task<EmployeeResponse> GetAllEmloyeeAsync(FilteringRequest requirement);

        Task<Employee> GetEmployeeByIdAsync(int employeeId);

        Task<Employee> AddEmployeeAsync(EmployeeRequest employeeRequest);

        Task<Employee> UpdateEmployeeAsync(Employee employee, EmployeeUpdateRequest employeeRequest);

        Task<Employee> UpdateEmployeePatchAsync(Employee employee, JsonPatchDocument employeeRequest);

        Task<Employee> DeleteEmployeeAsync(Employee employee);

        Task<bool> IsEmployeeExistsAsync(string email);
    }
}