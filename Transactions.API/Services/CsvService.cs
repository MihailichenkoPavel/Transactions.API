using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Transactions.API.DAL;
using Transactions.API.Interfaces;
using Transactions.API.Models;

namespace Transactions.API.Services
{
    public class CsvService : ICsvService
    {
        private readonly TransactionContext context;

        public CsvService(TransactionContext context)
        {
            this.context = context;
        }

        public void UploadCsv(IFormFile file)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                var csvListData = ReadCsv(file);
                foreach(var csvData in csvListData)
                {
                    Transaction existingTransaction = context.Transactions.Find(csvData.TransactionId);
                    if (existingTransaction == null)
                    {
                        context.Add(csvData);
                    }
                    else
                    {
                        context.Entry(existingTransaction).CurrentValues.SetValues(csvData);
                    }
                }

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT Transactions ON");
                context.SaveChanges();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Transactions OFF");

                transaction.Commit();
            }
        }

        private IEnumerable<Transaction> ReadCsv(IFormFile file)
        {
            var listTransactions = new List<Transaction>();
            using (var sreader = new StreamReader(file.OpenReadStream(), true))
            {
                //First line is header. If header is not passed in csv then we can neglect the below line.
                string[] headers = sreader.ReadLine().Split(',');
                //Loop through the records
                while (!sreader.EndOfStream)
                {
                    string[] rows = sreader.ReadLine().Split(',');
                    var tr = new Transaction
                    {
                        TransactionId = int.Parse(rows[0].ToString()),
                        Status = rows[1].ToString(),
                        Type = rows[2].ToString(),
                        ClientName = rows[3].ToString(),
                        Amount = decimal.Parse(rows[4].ToString().Remove(0, 1), CultureInfo.InvariantCulture)
                    };
                    listTransactions.Add(tr);
                }
            }
            return listTransactions;
        }
    }
}
