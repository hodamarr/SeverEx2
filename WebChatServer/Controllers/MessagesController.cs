using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebChatServer.Data;
using WebChatServer.Hubs;
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


        // GET: api/ofek/Contacts/5/messages
        [HttpGet("api/{userId}/contacts/{id}/messages/")]
        public async Task<IActionResult> GetMessages(string userId, string id)
        {
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
            return Json(await _context.Message.Where(Message => Message.ContactId == contact.Id).ToListAsync());
        }

        // GET: api/ofek/Contacts/5/messages/77
        [HttpGet("api/{userId}/contacts/{id}/messages/{msgId}")]
        public async Task<IActionResult> MessageDetails(string userId,string id, int msgId)
        {
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

        // POST: api/ofek/Contacts/5/messages
        [HttpPost("api/{userId}/contacts/{id}/messages/")]
        public async Task<IActionResult> CreateMessage(string userId, string id, [FromBody] MessageContent content)
        {
            if (ModelState.IsValid)
            {
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

        // Put: api/ofek/Contacts/5/messages/22
        [HttpPut("api/{userId}/contacts/{id}/messages/{msgId}")]
        public async Task<IActionResult> EditMessage(string userId, string id, int msgId, [FromBody] MessageContent content)
        {
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

        // DELETE: api/ofek/Contacts/5/messages/77
        [HttpDelete("api/{userId}/contacts/{id}/messages/{msgId}")]
        public async Task<IActionResult> DeleteMessage(string userId, string id, int msgId)
        {
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

        //POST : api/transfer
        [HttpPost("api/transfer/")]
        public async Task<IActionResult> TransferMsg([FromBody] TransferData data)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Name == data.From);
            if (user == null)
            {
                return NotFound();
            }
            var contact = await _context.Contact
            .FirstOrDefaultAsync(m => m.Id == data.To);
            if (contact == null)
            {
                return NotFound();
            }

            string address = "https://" + contact.Server + "/api/" + contact.Name + "/contacts/" + user.Name + "/messages";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(address);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"content\":\"" + data.Content + "\"}";

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

            private bool MessageExists(int id)
        {
            return (_context.Message?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
