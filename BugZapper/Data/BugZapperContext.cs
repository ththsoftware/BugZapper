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

        public DbSet<ProjectUser> ProjectUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Tickets)
                .WithOne(t => t.Project)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Tickets)
                .WithOne(t => t.User);
            modelBuilder.Entity<ProjectUser>()
                .HasKey(pu => new { pu.ProjectId, pu.UserId});
            modelBuilder.Entity<ProjectUser>()
                .HasOne(pu => pu.Project)
                .WithMany(p => p.ProjectUsers)
                .HasForeignKey(pu => pu.ProjectId);
            modelBuilder.Entity<ProjectUser>()
                .HasOne(pu => pu.User)
                .WithMany(u => u.ProjectUsers)
                .HasForeignKey(pu => pu.UserId);
        }
    }
}
