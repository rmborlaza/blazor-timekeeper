namespace Timekeeper.Models
{
    public class TransactionType
    {
        public int TransactionTypeId { get; set; }

        public string TransactionTypeName { get; set; }

        public List<TimekeepingTransaction> TimekeepingTransaction { get; set; }
    }
}
