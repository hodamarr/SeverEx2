using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Web.Http.Cors;
using WebChatServer.Data;
using WebChatServer.Models;
using System.Net.Http;
using System.Net;
using System.Security.Claims;

namespace WebChatServer.Controllers
{
    public class ContactsController : Controller
    {
        private readonly WebChatServerContext _context;

        public ContactsController(WebChatServerContext context)
        {
            _context = context;
        }


        // GET: api/Contacts
        [HttpGet("api/contacts/")]
        public async Task<IActionResult> Get()
        {
            string connected = HttpContext.Session.GetString("username");
            var user = await _context.User
            .FirstOrDefaultAsync(m => m.Name == connected);
            if (user == null)
            {
                return NotFound();
            }
            return Json(await _context.Contact.Where(Contact => Contact.UserName == user.Name).ToListAsync());
        }


        // GET: api/Contacts/id
        [HttpGet("api/contacts/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return Json(contact);
        }


        // POST: api/Contacts
        [HttpPost("api/contacts/")]
        public async Task<IActionResult> Create([FromBody] AddContactData data)
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userName = claimsIdentity.FindFirst("UserId")?.Value;
                //string connected = HttpContext.Session.GetString("username");
                User user = await _context.User
                .FirstOrDefaultAsync(m => m.Name == userName);
                Contact contact = new(data.Id, data.Name, data.Server, user);
                _context.Contact.Add(contact);
                _context.Update(user);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                return Created(string.Format("/api/contacts/{0}", contact.Id), null);
            }
            return BadRequest();

         
        }


        // Put: api/Contacts/5
        [HttpPut("api/contacts/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [FromBody] EditContactData contact)
        {
            var toChange = await _context.Contact.FindAsync(id);

            if (toChange == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    toChange.Name = contact.Name;
                    toChange.Server = contact.Server;
                    _context.Contact.Update(toChange);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(toChange.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return NoContent();
            }
            return BadRequest();
        }

        // GET: Contacts/Delete/5
        [HttpDelete("api/contacts/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }
            _context.Contact.Remove(contact);
            await _context.SaveChangesAsync();
            return Ok();
        }

        //POST : api/invitation
        [HttpPost("api/invitations/")]
        public async Task<IActionResult> Invite([FromBody] InvitationData data)
        {
            string address = "http/" + data.Server + "/api/contacts/";
            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Name == data.From);
            if (user == null)
            {
                return NotFound();
            }

            Dictionary<string, string> content = new Dictionary<string, string> {
                { "Name", user.Name },
                { "Nick", user.Nick },
                { "Server", user.Server },
            };

            HttpClient client = new HttpClient();
            var payload = new FormUrlEncodedContent(content);
            var response = await client.PostAsync(address, payload);
            if (response.StatusCode == HttpStatusCode.Created) 
            {
                return Created("", response.Content);
            }
            return NotFound();
        }

        private bool ContactExists(string id)
        {
            return (_context.Contact?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
