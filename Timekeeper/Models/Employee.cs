using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Timekeeper.Models
{
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    [ValidatableType]
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

        public List<TimekeepingTransaction> TimeKeepingTransactions { get; set; } = new();

        public Address Address { get; set; } = new();


        // User Account Auth
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public AccountType AccountType { get; set; }
    }

    // JUST NOTES. IGNORE
    // Logger using NLog

    // Mark employees as late/undertime
    // Count hours logged in a day

    // User Auth
    // Role-based access control
}
