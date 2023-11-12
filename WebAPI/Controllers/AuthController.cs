using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebAPI.DbContexts;
using WebAPI.Models.DTO;
using WebAPI.Models.Users;
using WebAPI.Utils;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private ApplicationContext _context;

        public AuthController()
        {
            _context = new ApplicationContext();
        }


        [HttpPost("/register")]
        public IActionResult Register([FromBody]RegisterRequestDto registerRequest)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == registerRequest.Login);
            if(user != null)
            {
                return BadRequest("User with this login already exist");
            }
            _context.Users.Add(new User
            {
                Login = registerRequest.Login,
                Password = AuthUtils.HashPassword(registerRequest.Password),
                Role = "user"
            });
            _context.SaveChanges();
            return Ok();
        }


        [HttpPost("/login")]
        public IActionResult Login([FromBody] LoginRequestDto loginRequest)
        {
            var identity = GetIdentity(loginRequest.Login, loginRequest.Password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password!"});
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                usename = identity.Name
            };

            return Ok(JsonConvert.SerializeObject(response));
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == username);
            if(user == null || !AuthUtils.VerifyPassword(password, user.Password))
            {
                return null;
            }

            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role),
                };

            var claimsIdentity
                = new ClaimsIdentity(claims, "Token",
                        ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
