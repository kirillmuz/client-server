using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.DbContexts;
using WebAPI.Models.Departments;
using WebAPI.Models.DTO;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class DepartmentsController : ControllerBase
    {
        private ApplicationContext _context;

        public DepartmentsController()
        {
            _context = new ApplicationContext();
        }

        [HttpGet]
        [Authorize(Roles = "admin, manager")]
        public IEnumerable<Department> GetDepartments()
        {
            return _context.Departments
                .Include(d => d.Employees).ThenInclude(e => e.Educations)
                .Include(d => d.Employees).ThenInclude(e => e.WorkExperience)
                .Include(d => d.Employees).ThenInclude(e => e.UserFiles)
                .ToList();
        }

        [HttpPost("department")]
        [Authorize(Roles = "admin")]
        public IActionResult AddDepartment([FromBody]AddDepartmentRequestDto addDepartmentDto)
        {
            var department = _context.Departments.Add(new Department { Name = addDepartmentDto.Name, Description = addDepartmentDto.Description});
            _context.SaveChanges();

            return Ok(department.Entity?.Id ?? 0);
        }

        [HttpPut("department")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateDepartment([FromBody]UpdateDepartmentRequestDto updateDepartmentDto)
        {
            var department = _context.Departments.FirstOrDefault(d => d.Id == updateDepartmentDto.Id);
            if(department != null)
            {
                department.Name = updateDepartmentDto.Name;
                department.Description = updateDepartmentDto.Description;
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest("Department not found");
        }

        [HttpDelete("department")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteDepartment(int id)
        {
            var department = _context.Departments.FirstOrDefault(d => d.Id == id);
            if(department != null )
            {
                _context.Departments.Remove(department);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest("Department not found");
        }
    }
}
