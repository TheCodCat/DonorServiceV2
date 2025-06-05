using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Database
{
    public class DiliveryContext : DbContext
    {
        public DbSet<Donor> Donors { get; set; } = null!;
        public DbSet<Record> Records { get; set; } = null!;
        public DbSet<DiliveryPoint> DiliveryPoints { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=dilivery.db");
        }
    }
}
