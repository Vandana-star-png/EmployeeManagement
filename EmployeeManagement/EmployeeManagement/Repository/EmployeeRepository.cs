using System.Diagnostics.Eventing.Reader;
using AutoMapper;
using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Http.HttpResults;

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

        public async Task<List<Employees>> GetAllEmployeesAsync()
        {
            var employees = await _context.Employees.ToListAsync();
            return employees;
        }

        public async Task<EmployeeModel> GetEmployeeByIdAsync(int employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            return _mapper.Map<EmployeeModel>(employee);
        }

        public async Task<int> AddEmployeeAsync(EmployeeModel employeeModel)
        {
            var employee = new Employees()
            {
                Name = employeeModel.Name,
                Salary = employeeModel.Salary,
                Location = employeeModel.Location,
                Email = employeeModel.Email,
                Department = employeeModel.Department,
                Qualification = employeeModel.Qualification,
                CreatedDate = DateTime.Now,
            };

            _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            return employeeModel.Id;
        }

        public async Task UpdateEmployeeAsync(int id, EmployeeModel employeeModel)
        {
            var employee = new Employees()
            {
                Name = employeeModel.Name,
                Salary = employeeModel.Salary,
                Location = employeeModel.Location,
                Email = employeeModel.Email,
                Department = employeeModel.Department,
                Qualification = employeeModel.Qualification,
                UpdatedDate = DateTime.UtcNow
            };

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEmployeePatchAsync(int id, JsonPatchDocument employeeModel)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                employeeModel.ApplyTo(employee);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteEmployeeAsync(int employeeId)
        {
            var employee = new Employees() { Id = employeeId };

            _context.Employees.Remove(employee);

            await _context.SaveChangesAsync();
        }
    }
}
