
using AdminLTE1.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE1.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<MenuPermission> MenuPermissions { get; set; }

        public DbSet<Country> Country { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<City> City { get; set; }



       
    }
}
