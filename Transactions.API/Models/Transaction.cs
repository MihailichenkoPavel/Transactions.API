namespace Transactions.API.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string ClientName { get; set; }
        public decimal Amount { get; set; }
    }
}
