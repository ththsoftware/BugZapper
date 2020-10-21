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

        public async Task<IActionResult> Index()
        {
            var bugZapperContext = _context.Ticket.Include(t => t.Project).Include(t => t.User);
            return View(await bugZapperContext.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
