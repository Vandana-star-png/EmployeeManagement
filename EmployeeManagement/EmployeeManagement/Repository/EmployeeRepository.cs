using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using AutoMapper;
using EmployeeManagement.Caching;
using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EmployeeManagement.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IOptions<SortSettings> _options;
        private readonly PagingSettings _pagingSettings;

        public EmployeeRepository(EmployeeDbContext context, IMapper mapper, ICacheService cacheService,
            IOptions<SortSettings> options, IOptions<PagingSettings> pagingSettings)
        {
            _context = context;
            _mapper = mapper;
            _cacheService = cacheService;
            _options = options;
            _pagingSettings = pagingSettings.Value;
        }

        public async Task<EmployeeResponse> GetAllEmloyeeAsync(FilteringRequest request)
        {
            string cacheKey = CacheKeys.Employee(request, _pagingSettings.DefaultPage, _pagingSettings.DefaultPageSize);

            var cachedEmployee = await _cacheService.GetAllAsync(cacheKey);

            if (cachedEmployee != null)
            {
                return cachedEmployee;
            }

            IQueryable<Employee> employees = _context.Employees;
            employees = SearchEmployee(employees, request.Term);
            employees = SortEmployee(employees, request.Sort);

            int finalPage = request.Page > 0 ? request.Page : _pagingSettings.DefaultPage;
            int finalPageSize = request.PageSize > 0 ? request.PageSize : _pagingSettings.DefaultPageSize;

            var employeeResponse = await ApplyPagination(employees, finalPage, finalPageSize);

            await _cacheService.SetAllAsync(cacheKey, employeeResponse);
            return employeeResponse;
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            string cacheKey = CacheKeys.EmployeeById(id);

            var employee = await _cacheService.GetAsync(cacheKey);

            if (employee == null)
            {
                employee = await _context.Employees.FindAsync(id);
                if (employee != null)
                {
                    await _cacheService.SetAsync(cacheKey, employee);
                }
            }
            return employee;
        }

        public async Task<Employee> AddEmployeeAsync(EmployeeRequest employeeRequest)
        {
            var employee = _mapper.Map<Employee>(employeeRequest);
            employee.CreatedDate = DateTime.Now;

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            await _cacheService.RemoveAllAsync();

            return employee;
        }

        public async Task<Employee> UpdateEmployeeAsync(Employee employee, EmployeeUpdateRequest employeeRequest)
        {
            _mapper.Map(employeeRequest, employee);

            employee.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            await _cacheService.RemoveAsync(CacheKeys.EmployeeById(employee.Id));
            await _cacheService.RemoveAllAsync();

            return employee;
        }

        public async Task<Employee> UpdateEmployeePatchAsync(Employee employee, JsonPatchDocument employeeRequest)
        {
            employee.UpdatedDate = DateTime.UtcNow;

            employeeRequest.ApplyTo(employee);

            await _context.SaveChangesAsync();

            await _cacheService.RemoveAsync(CacheKeys.EmployeeById(employee.Id));
            await _cacheService.RemoveAllAsync();

            return employee;
        }

        public async Task<Employee> DeleteEmployeeAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            await _cacheService.RemoveAsync(CacheKeys.EmployeeById(employee.Id));
            await _cacheService.RemoveAllAsync();

            return employee;
        }

        public async Task<bool> IsEmployeeExistsAsync(string email)
        {
            var isEmailExists = await _context.Employees.AnyAsync(e => e.Email == email);
            return isEmailExists;
        }

        private IQueryable<Employee> SearchEmployee(IQueryable<Employee> employees, string? term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                employees = _context.Employees;
            }
            else
            {
                term = term.Trim().ToLower();

                employees = _context.Employees.Where(e => e.Name.ToLower().Contains(term) ||
                e.Email.ToLower().Contains(term) || e.Location.ToLower().Contains(term));
            }
            return employees;
        }

        private IQueryable<Employee> SortEmployee(IQueryable<Employee> employees, string? sort)
        {
            if (!string.IsNullOrWhiteSpace(sort))
            {
                var sortFields = sort.Split(',');
                StringBuilder orderQueryBuilder = new StringBuilder();

                PropertyInfo[] propertyInfo = typeof(Employee).GetProperties();

                foreach (var field in sortFields)
                {
                    var sortField = field.Trim();

                    var property = propertyInfo.FirstOrDefault(a => a.Name.Equals(sortField, StringComparison.OrdinalIgnoreCase));
                    if (property == null)
                        continue;
                    orderQueryBuilder.Append($"{property.Name.ToString()} {_options.Value.DefaultSortOrder}, ");
                }

                string orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

                if (!string.IsNullOrWhiteSpace(orderQuery))
                {
                    employees = employees.OrderBy(orderQuery);
                }
                else
                {
                    employees = employees.OrderBy(a => a.Id);
                }
            }
            return employees;
        }

        private async Task<EmployeeResponse> ApplyPagination(IQueryable<Employee> employees, int page, int limit)
        {
            var totalCount = await employees.CountAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)limit);
           
            var pagedEmployees = await employees
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            var pagedEmployeeData = new EmployeeResponse
            {
                Employees = pagedEmployees,
                TotalCount = totalCount,
                TotalPages = totalPages,
            };
            return pagedEmployeeData;
        }
    }
}
