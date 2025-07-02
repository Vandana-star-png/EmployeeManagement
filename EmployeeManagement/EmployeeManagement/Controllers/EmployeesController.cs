using EmployeeManagement.Models;
using EmployeeManagement.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

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
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById([FromRoute]int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewEmployee([FromBody]EmployeeModel employeeModel)
        {
            var id = await _employeeRepository.AddEmployeeAsync(employeeModel);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }
            return CreatedAtAction(nameof(GetEmployeeById), new {id = id, controller = "employees"}, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody]EmployeeModel employeeModel)
        {
            if(id != employeeModel.Id)
            {
                return BadRequest("ID mismatch");
            }

            await _employeeRepository.UpdateEmployeeAsync(id, employeeModel);
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateEmployeePatch(int id, [FromBody] JsonPatchDocument employeeModel)
        {
            await _employeeRepository.UpdateEmployeePatchAsync(id, employeeModel);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute]int id)
        {
            await _employeeRepository.DeleteEmployeeAsync(id);
            return NotFound();
        }
    }
}
