using Microsoft.EntityFrameworkCore;
using NakedBank.Infrastructure.Entities;

namespace NakedBank.Infrastructure
{
    public class NakedContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }

        public DbSet<Balance> Balances { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public NakedContext(DbContextOptions<NakedContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .HasIndex(b => b.Timestamp);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
