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


        // GET: api/ofek/Contacts
        [HttpGet("api/{userName}/contacts/")]
        public async Task<IActionResult> Get(string userName)
        {
           // string connected = HttpContext.Session.GetString("username");
            var user = await _context.User
            .FirstOrDefaultAsync(m => m.Name == userName);
            if (user == null)
            {
                return NotFound();
            }
            return Json(await _context.Contact.Where(Contact => Contact.UserName == user.Name).ToListAsync());
        }


        // GET: api/ofek/Contacts/5
        [HttpGet("api/{userName}/contacts/{id}")]
        public async Task<IActionResult> Details(string id, string userName)
        {
            if (id == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null || contact.UserName != userName)
            {
                return NotFound();
            }

            return Json(contact);
        }


        // POST: api/ofek/Contacts
        [HttpPost("api/{userName}/contacts")]
        public async Task<IActionResult> Create(string userName, [FromBody] AddContactData data)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.User.FirstOrDefaultAsync(m => m.Name == userName);
                if (user == null) return NotFound();
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


        // Put: api/ofek/Contacts/5
        [HttpPut("api/{userName}/contacts/{id}")]
        public async Task<IActionResult> Edit(string id, string userName, [FromBody] EditContactData contact)
        {
            var toChange = await _context.Contact.FindAsync(id);

            if (toChange == null || toChange.UserName != userName)
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

        // GET: Contacts/ofek/Delete/5
        [HttpDelete("api/{userId}/contacts/{id}")]
        public async Task<IActionResult> Delete(string userId, string id)
        {
            if (id == null || _context.Contact == null)
            {
                return NotFound();
            }
            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Name == userId);
            if (user == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null || contact.UserName != user.Name)
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
            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Name == data.From);
            if (user == null)
            {
                return NotFound();
            }

            string address = "https://" + data.Server + "/api/" + data.To + "/contacts/";


            var httpWebRequest = (HttpWebRequest)WebRequest.Create(address);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"id\":\"" + user.Name + "\"," +
                              "\"name\":\"" + user.Name + "\"," +
                              "\"server\":\"" + user.Server + "\"}";

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return Ok(result);
            }
            return NotFound();
        }

        private bool ContactExists(string id)
        {
            return (_context.Contact?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
