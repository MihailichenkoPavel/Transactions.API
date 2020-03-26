using Microsoft.EntityFrameworkCore;
using Transactions.API.Models;

namespace Transactions.API.DAL
{
    public class TransactionContext : DbContext
    {
        public TransactionContext(DbContextOptions<TransactionContext> options) : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }
    }
}