using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.DbContexts;
using WebAPI.Models.Departments;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private ApplicationContext _context;

        public DepartmentsController()
        {
            _context = new ApplicationContext();
        }

        [HttpGet]
        public IEnumerable<Department> GetDepartments()
        {
            return _context.Departments
                .Include(d => d.Employees).ThenInclude(e => e.Educations)
                .Include(d => d.Employees).ThenInclude(e => e.WorkExperience)
                .ToList();
        }

        [HttpPost("department")]
        public IActionResult AddDepartment(string name, string? description)
        {
            var department = _context.Departments.Add(new Department { Name = name, Description = description });
            _context.SaveChanges();

            return Ok(department.Entity?.Id ?? 0);
        }

        [HttpPut("department")]
        public IActionResult UpdateDepartment(int id, string name, string? description)
        {
            var department = _context.Departments.FirstOrDefault(d => d.Id == id);
            if(department != null)
            {
                department.Name = name;
                department.Description = description;
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest("Department not found");
        }

        [HttpDelete("department")]
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
