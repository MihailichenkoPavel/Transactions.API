using System.Collections.Generic;
using Transactions.API.Models;

namespace Transactions.API.Interfaces
{
    public interface ITransactionRepository
    {
        IEnumerable<Transaction> GetAll();
        Transaction GetById(int id);
        void Delete(int id);
        void Update(Transaction item);

        IEnumerable<Transaction> GetFilteredTransactions(string status, string type);
    }
}
