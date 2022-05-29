using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebChatServer.Data;
using WebChatServer.Models;

namespace WebChatServer.Controllers
{
    public class UsersController : Controller
    {
        private readonly WebChatServerContext _context;
        public IConfiguration _configuration;

        public UsersController(WebChatServerContext context, IConfiguration config)
        {
            _context = context;
            _configuration = config;
        }


        // GET: api/users
        [HttpGet("api/users/")]
        public async Task<IActionResult> GetUsers()
        {
            return Json(await _context.User.ToListAsync());
        }

        // GET: api/users/id
        [HttpGet("api/users/{id}")]
        public async Task<IActionResult> UserDetails(string id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Name == id);
            if (user == null)
            {
                return NotFound();
            }

            return Json(user);
        }

        // POST: api/users
        [HttpPost("api/users/")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                //if user name is taken
                if (UserExists(user.Name)) return NoContent();
                else
                {
                    //HttpContext.Session.SetString("username", user.Name);
                    _context.Add(user);
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    return Created(string.Format("/api/users/{0}", user.Name), null);
                }
            }
            return BadRequest();
        }

        // POST: api/users/login
        [HttpPost("api/users/login/")]
        public async Task<IActionResult> Login([FromBody] LoginData data)
        {
            var user = await _context.User
           .FirstOrDefaultAsync(m => m.Name == data.UserName);
            if (user == null) return BadRequest();
            else if (data.Password != user.Password) return BadRequest();
            else
            {
                //HttpContext.Session.SetString("username", user.Name);
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub , _configuration["JWTParams:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                    new Claim("UserID", user.Name)

                };
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTParams:SecretKey"]));
                var mac = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["JWTParams:Issuer"],
                    _configuration["JWTParams:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(20),
                    signingCredentials: mac);

                return Ok();
            }
        }

            private bool UserExists(string id)
        {
          return (_context.User?.Any(e => e.Name == id)).GetValueOrDefault();
        }
    }
}
