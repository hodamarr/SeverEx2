using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebChatServer.Data;

namespace WebChatServer.Controllers
{
    public class ContactsController : Controller
    {
        private readonly WebChatServerContext _context;

        public ContactsController(WebChatServerContext context)
        {
            _context = context;
        }


        // GET: Contacts
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (_context.Contact == null)
            {
                return NotFound();
            }
            return Json(await _context.Contact.ToListAsync());
        }


        // GET: Contacts/id
        [HttpGet("{id}")]
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.Contact == null)
        //    {
        //        return NotFound();
        //    }

        //    var contact = await _context.Contact
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (contact == null)
        //    {
        //        return NotFound();
        //    }

        //    return Json(contact);
        //}

        // GET: Contacts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
            }
            return BadRequest();
        }


        // GET: Contacts/Edit/5
        [HttpPut("{id}")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        // POST: api/Contacts/5
        [HttpPut]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Contact contact)
        //{
        //    if (id != contact.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(contact);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ContactExists(contact.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(contact);
        //}

        // GET: Contacts/Delete/5
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

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Contact == null)
            {
                return Problem("Entity set 'WebChatServerContext.Contact'  is null.");
            }
            var contact = await _context.Contact.FindAsync(id);
            if (contact != null)
            {
                _context.Contact.Remove(contact);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(string id)
        {
            return (_context.Contact?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
