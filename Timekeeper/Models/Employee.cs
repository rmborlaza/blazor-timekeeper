using System.ComponentModel.DataAnnotations;

namespace Timekeeper.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; } // Can be an enum. Will use string for simplicity.

        public DateTime DateHired { get; set; }

        public List<TimekeepingTransaction> TimeKeepingTransactions { get; } = new();
    }
}
