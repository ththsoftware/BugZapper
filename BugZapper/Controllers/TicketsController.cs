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
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!User.Identity.Name.Equals("Guest"))
                {
                    return new RedirectResult("/Home/OpenTickets");
                }
                else
                {
                    return new RedirectResult("/Guest/OpenTickets");
                }

            }
            else
            {
                return new RedirectResult("/Account/Login");
            }
            
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            string stringId = id.ToString();
            string guestPath = "/Guest/TicketDetails/" + stringId;
            var ticket = await _context.Ticket
                        .Include(t => t.Project)
                        .Include(t => t.User)
                        .FirstOrDefaultAsync(m => m.TicketId == id);
            if (User.Identity.IsAuthenticated)
            {
                if (!User.Identity.Name.Equals("Guest"))
                {
                    
                    if (ticket == null)
                    {
                        return NotFound();
                    }

                    return View(ticket);
                }
                else
                {
                    return new RedirectResult(guestPath);
                }

            }
            else
            {
                return new RedirectResult("/Account/Login");
            }
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectTitle");
                ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserName");
                return View();
            }
            else
            {
                return new RedirectResult("/Account/Login");
            }
            
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TicketId,TicketNumber,TicketSubject,TicketOwner,CreatedBy,ClosedDate,TicketStatus,BugDescription,ProjectId,UserId")] Ticket ticket)
        {
            if (User.Identity.IsAuthenticated)
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
                    return RedirectToAction("Index", "Home");
                }
                ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectTitle", ticket.ProjectId);
                ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserName", ticket.UserId);
                return View(ticket);

            }
            else
            {
                return new RedirectResult("/Account/Login");
            }
            
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            if (User.Identity.IsAuthenticated)
            {
                if (!User.Identity.Name.Equals("Guest"))
                {
                    
                    if (ticket == null)
                    {
                        return NotFound();
                    }
                    ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectTitle", ticket.ProjectId);
                    ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserName", ticket.UserId);
                    return View(ticket);
                }
                else
                {
                    return new RedirectResult("/Guest/OpenTickets");
                }

            }
            else
            {
                return new RedirectResult("/Account/Login");
            }
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TicketId,TicketNumber,TicketSubject,TicketOwner,CreatedBy,ClosedDate,TicketStatus,BugDescription,ProjectId,UserId")] Ticket ticket)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!User.Identity.Name.Equals("Guest"))
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
                        return RedirectToAction("Index", "Home");
                    }
                    ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectTitle", ticket.ProjectId);
                    ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserName", ticket.UserId);
                    return View(ticket);
                }
                else
                {
                    return new RedirectResult("/Guest/OpenTickets");
                }

            }
            else
            {
                return new RedirectResult("/Account/Login");
            }
            
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!User.Identity.Name.Equals("Guest"))
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
                else
                {
                    return new RedirectResult("/Guest/OpenTickets");
                }

            }
            else
            {
                return new RedirectResult("/Account/Login");
            }
            
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!User.Identity.Name.Equals("Guest"))
                {
                    var ticket = await _context.Ticket.FindAsync(id);
                    _context.Ticket.Remove(ticket);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return new RedirectResult("/Guest/OpenTickets");
                }

            }
            else
            {
                return new RedirectResult("/Account/Login");
            }
            
        }

        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.TicketId == id);
        }
    }
}
