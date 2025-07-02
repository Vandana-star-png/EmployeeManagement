using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace EmployeeManagement.Repository
{
    public interface IEmployeeRepository
    {
        Task<List<Employees>> GetAllEmployeesAsync();

        Task<EmployeeModel> GetEmployeeByIdAsync(int employeeId);

        Task<int> AddEmployeeAsync(EmployeeModel employeeModel);

        Task UpdateEmployeeAsync(int id, EmployeeModel employeeModel);

        Task UpdateEmployeePatchAsync(int id, JsonPatchDocument employeeModel);

        Task DeleteEmployeeAsync(int employeeId);
    }
}