using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BugZapper.Models;
using BugZapper.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using System.Globalization;

namespace BugZapper.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly BugZapperContext _context;

        /*public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        } */
        

        public HomeController(BugZapperContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // If the user is authenticated, then this is how you can get the access_token and id_token
            if (User.Identity.IsAuthenticated)
            {
                if (!User.Identity.Name.Equals("Guest"))
                {
                    return new RedirectResult("/Home/OpenTickets");
                } else
                {
                    return new RedirectResult("/Guest/OpenTickets");
                }
                 
            } else
            {
                return new RedirectResult("/Account/Login");
            }
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> OpenTickets()
        {
            // If the user is authenticated, then this is how you can get the access_token and id_token
            if (User.Identity.IsAuthenticated)
            {
                if (!User.Identity.Name.Equals("Guest"))
                {
                    var bugZapperContext = _context.Ticket.Include(t => t.Project).Include(t => t.User);
                    return View(await bugZapperContext.ToListAsync());
                } else
                {
                    return new RedirectResult("/Guest/OpenTickets");
                }
            } else
            {
                return new RedirectResult("/Account/Login");
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
