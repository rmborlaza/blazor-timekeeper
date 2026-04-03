namespace Timekeeper.Models
{
    public class DailyTimeRecordCollection : List<DailyTimeRecord>
    {
        public DailyTimeRecordCollection(List<TimekeepingTransaction> transactionRecords)
        {
            if (transactionRecords == null)
            {
                throw new ArgumentNullException(nameof(transactionRecords));
            }

            var records = transactionRecords.OrderBy(r => r.TransactionDateTime).ToList();

            foreach (var record in records)
            {
                AddTransaction(record);
            }
        }

        public void AddTransaction(TimekeepingTransaction transaction)
        {
            if (Count == 0 && transaction.TransactionType.TransactionTypeName == "OUT")
            {
                throw new InvalidOperationException("Cannot create the first DTR with a time OUT.");
            }

            if (transaction.TransactionType.TransactionTypeName == "IN")
            {
                if (Count > 0 && this[0].TimeOut == null)
                {
                    throw new InvalidOperationException("Cannot add time IN log while the latest DTR is not timed OUT.");
                }

                var dtr = new DailyTimeRecord(transaction);
                if (Count > 0)
                {
                    Insert(0, dtr);
                }
                else
                {
                    Add(dtr);
                }
            }

            else if (transaction.TransactionType.TransactionTypeName == "OUT")
            {
                this[0].TimeOut = transaction;
            }
        }

        public void AddRecord(DailyTimeRecord record)
        {

        }
    }
}
