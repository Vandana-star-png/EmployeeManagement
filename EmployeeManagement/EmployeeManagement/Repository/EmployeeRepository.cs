using AutoMapper;
using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;

namespace EmployeeManagement.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _context;
        private readonly IMapper _mapper;

        public EmployeeRepository(EmployeeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            var employees = await _context.Employees.ToListAsync();
            return employees;
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            return employee;
        }

        public async Task<Employee> AddEmployeeAsync(EmployeeRequest employeeRequest)
        {
            var employee = _mapper.Map<Employee>(employeeRequest);
            employee.CreatedDate = DateTime.Now;

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return employee;
        }

        public async Task<Employee> UpdateEmployeeAsync(Employee employee, EmployeeRequest employeeRequest)
        {
            _mapper.Map(employeeRequest, employee);

            employee.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateEmployeePatchAsync(Employee employee, JsonPatchDocument employeeRequest)
        {
            employee.UpdatedDate = DateTime.UtcNow;

            employeeRequest.ApplyTo(employee);

            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> DeleteEmployeeAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<bool> IsEmployeeExistsAsync(string email)
        {
            var isEmailExists = await _context.Employees.AnyAsync(e => e.Email == email);
            return isEmailExists;
        }
    }
}
