using System;
using System.Collections.Generic;
using System.Linq;
using Transactions.API.Interfaces;
using Transactions.API.Models;

namespace Transactions.API.DAL
{
    public class TransactionService : ITransactionService
    {
        private readonly TransactionContext context;

        public TransactionService(TransactionContext context)
        {
            this.context = context;
        }

        public IEnumerable<Transaction> GetAll()
        {
            return context.Transactions;
        }

        public Transaction GetById(int id)
        {
            return context.Transactions.Find(id);
        }

        public void Delete(int id)
        {
            var item = context.Transactions.Find(id);
            if (item == null)
                throw new ArgumentNullException("item");
            context.Transactions.Remove(item);
            context.SaveChanges();
        }

        public void Update(Transaction item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            var objItem = context.Transactions.Find(item.Id);
            if (objItem != null)
            {
                objItem.Status = item.Status;
            }
            context.SaveChanges();
        }

        public IEnumerable<Transaction> GetFilteredTransactions(string status, string type)
        {
            if (status != null && type != null)
            {
                return context.Transactions.Where(x => x.Status == status && x.Type == type);
            }
            else if (status != null)
            {
                return context.Transactions.Where(x => x.Status == status);
            }
            else if (type != null)
            {
                return context.Transactions.Where(x => x.Type == type);
            }
            else
            {
                return context.Transactions;
            }
        }
    }
}
