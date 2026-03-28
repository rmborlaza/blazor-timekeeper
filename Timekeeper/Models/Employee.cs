namespace Timekeeper.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public DateOnly DateHired { get; set; }

        public List<TimekeepingTransaction> TimeKeepingTransactions { get; } = new();
    }
}
