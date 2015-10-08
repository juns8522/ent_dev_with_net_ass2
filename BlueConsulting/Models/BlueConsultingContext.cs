using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BueConsulting.Models
{
    public class BlueConsultingContext : DbContext
    {
        public BlueConsultingContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Expense> Expenses { get; set; }
    }
}
