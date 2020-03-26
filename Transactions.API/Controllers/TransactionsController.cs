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

        /// <summary>
        /// This is transaction XML documentation: Post method
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/transactions/importcsv
        /// </remarks>
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

        /// <summary>
        /// This is transaction XML documentation: Get method
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/transactions/all
        /// </remarks>
        [HttpGet]
        [Route("all")]
        public IEnumerable<Transaction> All()
        {
            return _transactionService.GetAll();
        }

        /// <summary>
        /// This is transaction XML documentation: Get method
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/transactions/get/5
        /// </remarks>
        /// <param name="id"></param>
        [HttpGet]
        [Route("get/{id}")]
        public Transaction Get(int id)
        {
            return _transactionService.GetById(id);
        }

        /// <summary>
        /// This is transaction XML documentation: Put method
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/transactions/update
        ///     {
        ///        "Id": 1,
        ///        "TransactionId": 1,
        ///        "Status": "Pending",
        ///        "Type": "Withdrawal",
        ///        "Amount": 28.43
        ///     }
        ///
        /// </remarks>
        /// <param name="transaction"></param>
        [HttpPut]
        [Route("update")]
        public void Update(Transaction transaction)
        {
            _transactionService.Update(transaction);
        }

        /// <summary>
        /// This is transactions XML documentation: Delete method
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE api/transactions/delete/5
        /// </remarks>
        /// <param name="id">value that delete</param>
        [HttpDelete]
        [Route("delete/{id}")]
        public void Delete(int id)
        {
            _transactionService.Delete(id);
        }

        /// <summary>
        /// This is transactions XML documentation: Post method
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/transactions/filtered/?status=Pending&amp;type=Refill
        /// </remarks>
        /// <param name="status">filtered value</param>
        /// <param name="type">filtered value</param>
        [HttpGet]
        [Route("filtered")]
        public IEnumerable<Transaction> GetFilteredTransactions(string status, string type)
        {
            return _transactionService.GetFilteredTransactions(status, type);
        }
    }
}