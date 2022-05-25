using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebChatServer.Data;
using WebChatServer.Models;

namespace WebChatServer.Controllers
{
    public class MessagesController : Controller
    {
        private readonly WebChatServerContext _context;

        public MessagesController(WebChatServerContext context)
        {
            _context = context;
        }


        // GET: api/Contacts/5/messages
        [HttpGet("api/contacts/{id}/messages")]
        public async Task<IActionResult> GetMessages(string id)
        {
            var contact = await _context.Contact
            .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }
            List<Message> messages = await _context.Message.ToListAsync();
            return Json(await _context.Message.Where(Message => Message.ContactId == contact.Id).ToListAsync());
        }

        // GET: api/Contacts/5/messages/77
        [HttpGet("api/contacts/{id}/messages/{msgId}")]
        public async Task<IActionResult> MessageDetails(string id, int msgId)
        {
            var contact = await _context.Contact
            .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }
            if (id == null || _context.Message == null)
            {
                return NotFound();
            }
            var message = await _context.Message.Where(Message => Message.ContactId == contact.Id)
                .FirstOrDefaultAsync(m => m.Id == msgId);
            if (message == null)
            {
                return NotFound();
            }

            return Json(message);
        }

        // POST: api/Contacts/5/messages
        [HttpPost("api/contacts/{id}/messages")]
        public async Task<IActionResult> CreateMessage(string id, [FromBody] MessageContent content)
        {
            if (ModelState.IsValid)
            {
                var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.Id == id);
                if (contact == null)
                {
                    return NotFound();
                }
                Message m = new(content.Content, true, contact.Id);
                _context.Add(m);
                contact.Lastdate = m.Created;
                contact.Last = m.Content;
                _context.Update(contact);
                try
                {
                    await _context.SaveChangesAsync();
                    return Created(string.Format("/api/Contacts/{0}/messages/{1}", contact.Id, m.Id), null);
                }
                catch (Exception e)
                {
                    return BadRequest();
                }

                //return RedirectToAction(nameof(Index));
            }
            return BadRequest();
        }

        // Put: api/Contacts/5
        [HttpPut("api/contacts/{id}/messages/{msgId}")]
        public async Task<IActionResult> EditMessage(string id, int msgId, [FromBody] MessageContent content)
        {
            var contact = await _context.Contact
            .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }
            if (id == null || _context.Message == null)
            {
                return NotFound();
            }
            var toChange = await _context.Message.Where(Message => Message.ContactId == contact.Id)
                .FirstOrDefaultAsync(m => m.Id == msgId);
            if (toChange == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    toChange.Content = content.Content;
                    _context.Message.Update(toChange);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(toChange.Id))
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

        // DELETE: api/Contacts/5/messages/77
        [HttpDelete("api/contacts/{id}/messages/{msgId}")]
        public async Task<IActionResult> DeleteMessage(string id, int msgId)
        {
            var contact = await _context.Contact
            .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }
            if (id == null || _context.Message == null)
            {
                return NotFound();
            }
            var message = await _context.Message.Where(Message => Message.ContactId == contact.Id)
                .FirstOrDefaultAsync(m => m.Id == msgId);
            if (message == null)
            {
                return NotFound();
            }
            if (_context.Message == null)
            {
                return NotFound();
            }
            _context.Message.Remove(message);
            await _context.SaveChangesAsync();
            return Ok();

        }

        private bool MessageExists(int id)
        {
            return (_context.Message?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
