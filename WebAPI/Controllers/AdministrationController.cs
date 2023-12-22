using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.DbContexts;
using WebAPI.Models.DTO;
using WebAPI.Models.Employees;
using WebAPI.Models.Users;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AdministrationController : ControllerBase
    {
        private ApplicationContext _context;
        public AdministrationController()
        {
            _context = new ApplicationContext();
        }

        [HttpGet("roles")]
        [Authorize(Roles = "admin")]
        public IEnumerable<string> GetRoles()
        {
            return new List<string>{
                "admin",
                "manager",
                "user"
            };
        }

        [HttpGet("getusers")]
        [Authorize(Roles = "admin")]
        public IActionResult GetUsers()
        {
            try
            {
                var users = _context.Users.ToList().OrderBy(u=>u.Id);
                return Ok(users);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("setuserrole")]
        [Authorize(Roles = "admin")]
        public IActionResult SetRoleToUser([FromBody] SetRoleRequestDto setRoleRequestDto)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u=>u.Id == setRoleRequestDto.UserId);
                if (user == null)
                {
                    return BadRequest("User not found");
                }
                user.Role = setRoleRequestDto.RoleName;
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
    }
}
