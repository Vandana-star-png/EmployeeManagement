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

        Task<bool> UpdateEmployeeAsync(int id, EmployeeModel employeeModel);

        Task<bool> UpdateEmployeePatchAsync(int id, JsonPatchDocument employeeModel);

        Task<bool> DeleteEmployeeAsync(int employeeId);
    }
}