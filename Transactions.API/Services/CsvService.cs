using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
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
        private readonly TransactionContext _context;

        public CsvService(TransactionContext context)
        {
            _context = context;
        }

        public void UploadCsv(IFormFile file)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var csvListData = ReadCsv(file);
                foreach (var csvData in csvListData)
                {
                    object[] parameters = {
                        new SqlParameter("@TransactionId", csvData.TransactionId),
                        new SqlParameter("@Status", csvData.Status),
                        new SqlParameter("@Type", csvData.Type),
                        new SqlParameter("@ClientName", csvData.ClientName),
                        new SqlParameter("@Amount", csvData.Amount)
                    };

                    _context.Database.ExecuteSqlCommand("InsertOrUpdateTransactions @TransactionId, @Status, @Type, @ClientName, @Amount", parameters);
                }

                _context.SaveChanges();

                transaction.Commit();
            }
        }

        private IEnumerable<Transaction> ReadCsv(IFormFile file)
        {
            var records = new List<Transaction>(); ;
            using (var reader = new StreamReader(file.OpenReadStream(), true))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var record = new Transaction
                    {
                        TransactionId = csv.GetField<int>("TransactionId"),
                        Status = csv.GetField("Status"),
                        Type = csv.GetField("Type"),
                        ClientName = csv.GetField("ClientName"),
                        Amount = decimal.Parse(csv.GetField("Amount").Remove(0, 1), CultureInfo.InvariantCulture)
                    };
                    records.Add(record);
                }
            }
            return records;
        }
    }
}