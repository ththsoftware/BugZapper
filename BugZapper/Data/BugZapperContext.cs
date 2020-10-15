using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BugZapper.Models;
using System.Security.Permissions;

namespace BugZapper.Data
{
    public class BugZapperContext : DbContext
    {
        public BugZapperContext (DbContextOptions<BugZapperContext> options) : base(options)
        {

        }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<User> User { get; set; }
    }
}
