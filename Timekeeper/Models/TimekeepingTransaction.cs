namespace Timekeeper.Models
{
    public class TimekeepingTransaction
    {
        public int TimeKeepingTransactionId { get; set; }

        public DateTime TransactionDateTime { get; set; }

        public int TransactionTypeId { get; set; }
        public TransactionType TransactionType { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
