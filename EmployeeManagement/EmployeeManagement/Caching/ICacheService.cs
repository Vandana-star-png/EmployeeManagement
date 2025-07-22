using EmployeeManagement.Data;
using EmployeeManagement.Models;

namespace EmployeeManagement.Caching
{
    public interface ICacheService
    {
        Task<EmployeeResponse> GetAllAsync(string cacheKey);

        Task SetAllAsync(string cacheKey, EmployeeResponse employeeResponse);

        Task<Employee> GetAsync(string cacheKey);

        Task SetAsync(string cacheKey, Employee employee);

        Task RemoveAsync(string cacheKey);

        Task RemoveAllAsync();
    }
}