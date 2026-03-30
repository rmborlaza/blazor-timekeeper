using System.ComponentModel.DataAnnotations.Schema;

namespace Timekeeper.Models
{
    public class TimekeepingTransaction
    {
        public int TimeKeepingTransactionId { get; set; }

        public DateTime TransactionDateTime { get; set; }

        [NotMapped]
        public DateTime TransactionDateTimeLocal
        {
            get => TransactionDateTime.ToLocalTime();
        }

        public int TransactionTypeId { get; set; }
        public TransactionType TransactionType { get; set; }
        // TransactionType can be an enum instead. Will use this class for the assessment.

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
