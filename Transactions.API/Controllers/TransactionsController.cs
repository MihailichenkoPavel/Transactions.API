using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Transactions.API.DAL;
using Transactions.API.Interfaces;
using Transactions.API.Models;
using Transactions.API.Services;

namespace Transactions.API.Controllers
{
    [EnableCors]
    [Route("api/transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ICsvService _csvService;
        private readonly ITransactionService _transactionService;
        private readonly TransactionContext _context;

        public TransactionsController(TransactionContext context)
        {
            _context = context;
            _csvService = new CsvService(_context);
            _transactionService = new TransactionService(_context);
        }

        [Route("importcsv")]
        [HttpPost]
        public void ImportCsv()
        {
            var file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                _csvService.UploadCsv(file);
            }

        }

        [HttpGet]
        [Route("all")]
        public IEnumerable<Transaction> All()
        {
            return _transactionService.GetAll();
        }

        [HttpGet]
        [Route("get/{id}")]
        public Transaction Get(int id)
        {
            return _transactionService.GetById(id);
        }

        [HttpPut]
        [Route("update")]
        public void Update(Transaction transaction)
        {
            _transactionService.Update(transaction);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public void Delete(int id)
        {
            _transactionService.Delete(id);
        }

        [HttpGet]
        [Route("filtered")]
        public IEnumerable<Transaction> GetFilteredTransactions(string status, string type)
        {
            return _transactionService.GetFilteredTransactions(status, type);
        }
    }
}