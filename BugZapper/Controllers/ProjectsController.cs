﻿using System;
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
    public class ProjectsController : Controller
    {
        private readonly BugZapperContext _context;

        public ProjectsController(BugZapperContext context)
        {
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return View(await _context.Project.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!User.Identity.Name.Equals("Guest"))
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
                }
                else
                {
                    return new RedirectResult("/Guest/ProjectTickets/" + id.ToString());
                }

            }
            else
            {
                return new RedirectResult("/Account/Login");
            }
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Name.Equals("tthompson"))
                {
                    return View();
                }
                else
                {
                    return new RedirectResult("/");
                }
            } else
            {
                return new RedirectResult("/Account/Login");
            }
            
            
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,ProjectTitle")] Project project)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Name.Equals("tthompson"))
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(project);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    return View(project);
                }
                else
                {
                    return new RedirectResult("/");
                }
            }
            else
            {
                return new RedirectResult("/Account/Login");
            }
            
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Name.Equals("tthompson"))
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var project = await _context.Project.FindAsync(id);
                    if (project == null)
                    {
                        return NotFound();
                    }
                    return View(project);
                }
                else
                {
                    return new RedirectResult("/");
                }
            }
            else
            {
                return new RedirectResult("/Account/Login");
            }
            
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,ProjectTitle")] Project project)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Name.Equals("tthompson"))
                {
                    if (id != project.ProjectId)
                    {
                        return NotFound();
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(project);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!ProjectExists(project.ProjectId))
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
                    return View(project);
                }
                else
                {
                    return new RedirectResult("/");
                }
            }
            else
            {
                return new RedirectResult("/Account/Login");
            }
            
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.ProjectId == id);

            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Name.Equals("tthompson"))
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    if (project == null)
                    {
                        return NotFound();
                    }

                    return View(project);
                }
                else
                {
                    return new RedirectResult("/");
                }
            }
            else
            {
                return new RedirectResult("/Account/Login");
            }
            
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Project.FindAsync(id);
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Name.Equals("tthompson"))
                {
                    _context.Project.Remove(project);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return new RedirectResult("/");
                }
            }
            else
            {
                return new RedirectResult("/Account/Login");
            }
            
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ProjectId == id);
        }
    }
}
