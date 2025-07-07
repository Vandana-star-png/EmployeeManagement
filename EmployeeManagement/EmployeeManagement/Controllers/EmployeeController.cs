using EmployeeManagement.Models;
using EmployeeManagement.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();

            if (employees == null || !employees.Any())
            {
                return NotFound("No Employees found");
            }

            return Ok(employees);
        }

        [HttpGet("{id}", Name = "GetEmployeeById")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            if(id <= 0)
            {
                return BadRequest("Employee ID must be positive number");
            }
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found");
            }

            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] EmployeeRequest employeeRequest)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Required employee data");
            }
            else if (await _employeeRepository.IsEmployeeExistsAsync(employeeRequest.Email))
            {
                return Conflict("Employee with this email already exists");
            }

            var id = await _employeeRepository.AddEmployeeAsync(employeeRequest);

            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);

            return CreatedAtRoute("GetEmployeeById", new { id = id }, employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] EmployeeRequest employeeRequest)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Employee data is required");
            }
            else if (await _employeeRepository.IsEmployeeExistsAsync(employeeRequest.Email))
            {
                return Conflict("Employee with this email already exists");
            }

            var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);

            if (existingEmployee == null)
            {
                return BadRequest($"Employee with ID {id} not found");
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
                return NotFound($"Employee with ID {id} not found");
            }

            var isUpdated = await _employeeRepository.UpdateEmployeePatchAsync(existingEmployee, employeeRequest);
            if (!isUpdated)
            {
                return StatusCode(500, "An error occurred while saving the updated employee data");
            }
            return Ok("Employee data updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (existingEmployee == null)
            {
                return BadRequest($"Employee with ID {id} not found");
            }

            var isDeleted = await _employeeRepository.DeleteEmployeeAsync(existingEmployee);
            if (!isDeleted)
            {
                return BadRequest($"Failed to delete employee with ID {id}");
            }
            return Ok($"Employee with ID {id} deleted successfully");
        }
    }
}
