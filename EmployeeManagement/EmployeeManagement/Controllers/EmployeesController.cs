using EmployeeManagement.Models;
using EmployeeManagement.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using EmployeeManagement.Data;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();
            
            if(employees == null || !employees.Any())
            {
                return NotFound("No Employees found");
            }

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById([FromRoute]int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
           
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found");
            }

            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewEmployee([FromBody]EmployeeModel employeeModel)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Required employee data");
            }

            var id = await _employeeRepository.AddEmployeeAsync(employeeModel);
            if (id == 0)
            {
                return BadRequest("Employee with this email already exists");
            }

            return CreatedAtAction(nameof(GetEmployeeById), new {id = id, controller = "employees"}, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee([FromBody]EmployeeModel employeeModel, [FromRoute]int id)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Employee data is required");
            }

            var isUpdated = await _employeeRepository.UpdateEmployeeAsync(id, employeeModel);

            if (!isUpdated)
            {
                return NotFound($"Employee with ID {id} not found");
            }
            return Ok("Employee data updated successfully");
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateEmployeePatch(int id, [FromBody] JsonPatchDocument employeeModel)
        {
            var isUpdated = await _employeeRepository.UpdateEmployeePatchAsync(id, employeeModel);
            if (!isUpdated)
            {
                return NotFound($"Employee with ID {id} not found");
            }
            return Ok("Employee data updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute]int id)
        {
            var isDeleted = await _employeeRepository.DeleteEmployeeAsync(id);
            if (!isDeleted)
            {
                return NotFound($"Employee with ID {id} not found");
            }
            return Ok($"Employee with ID {id} deleted successfully");
        }
    }
}
