using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugZapper.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugZapper.Controllers
{
    public class GuestController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly BugZapperContext _context;

        /*public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        } */


        public GuestController(BugZapperContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> OpenTickets()
        {
            var bugZapperContext = _context.Ticket.Include(t => t.Project).Include(t => t.User);
            return View(await bugZapperContext.ToListAsync());
        }

        public async Task<IActionResult> TicketDetails(int? id)
        {
            var ticket = await _context.Ticket
                        .Include(t => t.Project)
                        .Include(t => t.User)
                        .FirstOrDefaultAsync(m => m.TicketId == id);
            if (User.Identity.IsAuthenticated)
            {
                if (ticket == null)
                {
                    return NotFound();
                }

                return View(ticket);

            }
            else
            {
                return new RedirectResult("/Account/Login");
            }
        }

        public async Task<IActionResult> ProjectTickets(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var project = await _context.Project
                    .FirstOrDefaultAsync(m => m.ProjectId == id);
                if (project == null)
                {
                    return NotFound();
                }
                ViewBag.ProjectTitle = project.ProjectTitle;
                ViewBag.Id = project.ProjectId;
                return View(await _context.Ticket.ToListAsync());
            } else
            {
                return new RedirectResult("/Account/Login");
            }
        }
    }
}
