namespace Timekeeper.Models
{
    // This can be an enum instead, so there's one less table in the database.
    // Will use this for the assessment.
    public class TransactionType
    {
        public int TransactionTypeId { get; set; }

        public string TransactionTypeName { get; set; }

        //public List<TimekeepingTransaction> TimekeepingTransaction { get; set; }
    }
}
