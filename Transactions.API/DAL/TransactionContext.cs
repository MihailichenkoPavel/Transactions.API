using Microsoft.EntityFrameworkCore;
using Transactions.API.Models;

namespace Transactions.API.DAL
{
    public class TransactionContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }

        public TransactionContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TransactionsDB;Trusted_Connection=True;");
        }
    }
}