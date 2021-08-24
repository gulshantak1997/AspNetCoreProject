
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

        public DbSet<CMS> CMS { get; set; }

        public DbSet<Transaction> Transaction { get; set; }

        public DbSet<PayPalAPITransaction> PayPalAPITransaction { get; set; }


        public DbSet<Questions> Questions { get; set; }
        public DbSet<RelatedValues> RelatedValues { get; set; }

        public DbSet<FeedbackResult> FeedbackResult { get; set; }

        public DbSet<SurveyUrl> SurveyURL { get; set; }



    }
}
