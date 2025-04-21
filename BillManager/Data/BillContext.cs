using Microsoft.EntityFrameworkCore;
using BillManager.Models;

namespace BillManager.Data
{
    public class BillContext : DbContext
    {
        public BillContext(DbContextOptions<BillContext> options)
            : base(options)
        {
        }

        public DbSet<Bill> Bills { get; set; }
        public DbSet<Biller> Billers { get; set; } 
        public DbSet<Payment> Payments { get; set; }  // New DbSet for Payments

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add any necessary configurations, such as relationships
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Bill)
                .WithMany()
                .HasForeignKey(p => p.BillId);
    }
    }
}
