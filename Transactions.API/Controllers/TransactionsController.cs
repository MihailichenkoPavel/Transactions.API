using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
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
        private ICsvService csvService;
        private ITransactionRepository transactionRepository;

        public TransactionsController()
        {
            csvService = new CsvService(new TransactionContext());
            transactionRepository = new TransactionRepository(new TransactionContext());
        }
        [Route("importcsv")]
        [HttpPost]
        public void ImportCsv()
        {
            var file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                csvService.UploadCsv(file);
            }

        }

        [HttpGet]
        [Route("all")]
        public IEnumerable<Transaction> All()
        {
            return transactionRepository.GetAll();
        }

        [HttpGet]
        [Route("get/{id}")]
        public Transaction Get(int id)
        {
            return transactionRepository.GetById(id);
        }

        [HttpPut]
        [Route("update")]
        public void Update(Transaction transaction)
        {
            transactionRepository.Update(transaction);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public void Delete(int id)
        {
            transactionRepository.Delete(id);
        }

        [HttpGet]
        [Route("filtered")]
        public IEnumerable<Transaction> GetFilteredTransactions(string status, string type)
        {
            return transactionRepository.GetFilteredTransactions(status, type);
        }
    }
}