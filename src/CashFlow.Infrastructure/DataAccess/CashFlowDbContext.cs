using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess
{
    public class CashFlowDbContext : DbContext
    {
        public CashFlowDbContext(DbContextOptions options) :base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Expense> Expenses { get; set; }

   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tag>().ToTable("Tags");
        }

    }
}
