using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugZapper.Data;
using BugZapper.Models;

namespace BugZapper.Controllers
{
    public class TicketsController : Controller
    {
        private readonly BugZapperContext _context;
        private static int TicketEnumerator = 0;

        public TicketsController(BugZapperContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var bugZapperContext = _context.Ticket.Include(t => t.Project).Include(t => t.User);
            return View(await bugZapperContext.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.Project)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectTitle");
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserName");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TicketId,TicketNumber,TicketSubject,TicketOwner,CreatedBy,ClosedDate,TicketStatus,BugDescription,ProjectId,UserId")] Ticket ticket)
        {
            ticket.CreatedDate = DateTime.Now;
            if (ticket.TicketStatus.Equals("Closed"))
            {
                ticket.ClosedDate = DateTime.Now.ToString();
            }
            else
            {
                ticket.ClosedDate = "N/A";
            }
            ticket.TicketNumber = "T-" + (TicketEnumerator++);
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectTitle", ticket.ProjectId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserName", ticket.UserId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectTitle", ticket.ProjectId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserName", ticket.UserId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TicketId,TicketNumber,TicketSubject,TicketOwner,CreatedBy,ClosedDate,TicketStatus,BugDescription,ProjectId,UserId")] Ticket ticket)
        {
            if (id != ticket.TicketId)
            {
                return NotFound();
            }
            if (ticket.TicketStatus.Equals("Closed"))
            {
                ticket.ClosedDate = DateTime.Now.ToString();
            }
            else
            {
                ticket.ClosedDate = "N/A";
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.TicketId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectTitle", ticket.ProjectId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserName", ticket.UserId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.Project)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            _context.Ticket.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.TicketId == id);
        }
    }
}
