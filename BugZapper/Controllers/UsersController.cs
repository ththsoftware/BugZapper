using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugZapper.Data;
using BugZapper.Models;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.SignalR;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BugZapper.Controllers
{
    public class UsersController : Controller
    {
        private readonly BugZapperContext _context;

        public UsersController(BugZapperContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Name.Equals("tthompson"))
                {
                    
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
            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Name.Equals("tthompson"))
                {
                    if (id == null)
                    {
                        return NotFound();
                    }


                    if (user == null)
                    {
                        return NotFound();
                    }

                    return View(user);
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

        // GET: Users/Create
        public IActionResult Create()
        {
            var user = new User();
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Name.Equals("tthompson"))
                {
                    user.ProjectUsers = new List<ProjectUser>();
                    ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectTitle");
                    return View();
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

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("UserId,UserName,PermissionLevel,ProjectUsers")] User user, string[] selectedProject)
        {
            if (selectedProject != null)
            {
                user.ProjectUsers = new List<ProjectUser>();
                foreach (var project in selectedProject)
                {
                    var projectToAdd = new ProjectUser { UserId = user.UserId, ProjectId = int.Parse(project) };
                    user.ProjectUsers.Add(projectToAdd);
                }
            }
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Name.Equals("tthompson"))
                {
                    if (ModelState.IsValid)
                    {

                        _context.Add(user);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectTitle", selectedProject);
                    return View(user);
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

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _context.User.Include(u => u.ProjectUsers).ThenInclude(u => u.Project).AsNoTracking().FirstOrDefaultAsync(m => m.UserId == id);
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Name.Equals("tthompson"))
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    if (user == null)
                    {
                        return NotFound();
                    }
                    ViewData["ProjectIdRemaining"] = new SelectList(PopulateRemainingProjects(user), "ProjectId", "ProjectTitle");
                    return View(user);
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

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int? id, string[] selectedProjectAdd)
        {
            var user = await _context.User
                .Include(u => u.ProjectUsers)
                    .ThenInclude(u => u.Project)
                .FirstOrDefaultAsync(m => m.UserId == id);

            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Name.Equals("tthompson"))
                {
                    if (id == null)
                    {
                        return NotFound();
                    }



                    if (await TryUpdateModelAsync(
                        user,
                        "",
                        i => i.UserName, i => i.PermissionLevel))
                    {
                        UpdateProjectUsers(selectedProjectAdd, user);
                        try
                        {
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateException /* ex */)
                        {
                            //Log the error (uncomment ex variable name and write a log.)
                            ModelState.AddModelError("", "Unable to save changes. " +
                                "Try again, and if the problem persists, " +
                                "see your system administrator.");
                        }
                        return RedirectToAction(nameof(Index));
                    }
                    UpdateProjectUsers(selectedProjectAdd, user);
                    ViewData["ProjectIdRemaining"] = new SelectList(PopulateRemainingProjects(user), "ProjectId", "ProjectTitle", selectedProjectAdd);
                    return View(user);
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

        private void UpdateProjectUsers(string[] selectedProjectAdd, User user)
        {
            if (selectedProjectAdd == null)
            {
                user.ProjectUsers = new List<ProjectUser>();
                return;
            }

            var selectedProjectAddHS = new HashSet<string>(selectedProjectAdd);
            var projectUsers = new HashSet<int>
                (user.ProjectUsers.Select(p => p.Project.ProjectId));
            foreach (var project in _context.Project)
            {
                if (selectedProjectAddHS.Contains(project.ProjectId.ToString()))
                {
                    if (!projectUsers.Contains(project.ProjectId))
                    {
                        user.ProjectUsers.Add(new ProjectUser { UserId = user.UserId, ProjectId = project.ProjectId });
                    }
                }
                else
                {

                    if (projectUsers.Contains(project.ProjectId))
                    {
                        ProjectUser projectToRemove = user.ProjectUsers.FirstOrDefault(u => u.ProjectId == project.ProjectId);
                        _context.Remove(projectToRemove);
                    }
                }
            }
        }

        private List<Models.Project> PopulateRemainingProjects(User user)
        {
            var allProjects = _context.Project;
            var projectUsers = _context.ProjectUser;
            var userProjects = new HashSet<int>();
            var viewModel = new List<Models.Project>();
            bool isInUser = false;
            foreach (var projectUser in projectUsers)
            {
                if (user.UserId == projectUser.UserId)
                {
                    userProjects.Add(projectUser.ProjectId);
                }
            }
            foreach (var project in allProjects)
            {
                foreach (int projectId in userProjects)
                {
                    if (project.ProjectId == projectId) 
                    {
                        isInUser = true;
                        break;
                    }
                }
                if (!isInUser)
                {
                    viewModel.Add(project);
                }
                isInUser = false;   
            }
            return viewModel;
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Name.Equals("tthompson"))
                {
                    if (id == null)
                    {
                        return NotFound();
                    }


                    if (user == null)
                    {
                        return NotFound();
                    }

                    return View(user);
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

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Name.Equals("tthompson"))
                {
                    _context.User.Remove(user);
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

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserId == id);
        }
    }
}
