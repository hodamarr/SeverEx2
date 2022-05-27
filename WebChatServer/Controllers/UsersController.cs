using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebChatServer.Data;
using WebChatServer.Models;

namespace WebChatServer.Controllers
{
    public class UsersController : Controller
    {
        private readonly WebChatServerContext _context;

        public UsersController(WebChatServerContext context)
        {
            _context = context;
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
            return BadRequest();
        }

      
        private bool UserExists(string id)
        {
          return (_context.User?.Any(e => e.Name == id)).GetValueOrDefault();
        }
    }
}
