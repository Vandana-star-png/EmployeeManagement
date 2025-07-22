using EmployeeManagement.Models;
using EmployeeManagement.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using LazyCache;
using EmployeeManagement.Caching;
using EmployeeManagement.Data;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery]FilteringRequest filteringRequest)
        {
            var employeeResult = await _employeeRepository.GetAllEmloyeeAsync(filteringRequest);

            if (!employeeResult.Employees.Any())
            {
                return NotFound("No Employees found");
            }

            AddResponseHeaders(employeeResult);

            return Ok(employeeResult.Employees);

        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                return NotFound($"Employee ID {id} not found");
            }

            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] EmployeeRequest employeeRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (await _employeeRepository.IsEmployeeExistsAsync(employeeRequest.Email))
            {
                return Conflict("Employee with this email already exists");
            }

            var employee = await _employeeRepository.AddEmployeeAsync(employeeRequest);

            return Ok(employee);
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] EmployeeUpdateRequest employeeRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);

            if (existingEmployee == null)
            {
                return BadRequest($"Employee ID {id} not found");
            }

            var updatedEmployee = await _employeeRepository.UpdateEmployeeAsync(existingEmployee, employeeRequest);

            return Ok(updatedEmployee);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePatchAsync([FromBody] JsonPatchDocument employeeRequest, [FromRoute] int id)
        {
            if (employeeRequest == null)
            {
                return BadRequest("Patch document is required");
            }

            var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (existingEmployee == null)
            {
                return NotFound($"Employee ID {id} not found");
            }

            var isUpdated = await _employeeRepository.UpdateEmployeePatchAsync(existingEmployee, employeeRequest);

            return Ok("Employee data updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (existingEmployee == null)
            {
                return BadRequest($"Employee ID {id} not found");
            }

            var isDeleted = await _employeeRepository.DeleteEmployeeAsync(existingEmployee);

            return Ok($"Employee ID {id} deleted successfully");
        }

        private void AddResponseHeaders(EmployeeResponse employees)
        {
            Response.Headers.Add("X-Total-Count", employees.TotalCount.ToString());

            Response.Headers.Add("X-Total-Pages", employees.TotalPages.ToString());
        }
    }
}
