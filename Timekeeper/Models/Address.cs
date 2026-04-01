using System.ComponentModel.DataAnnotations;

namespace Timekeeper.Models
{
    [ValidatableType]
    public class Address
    {
        public int AddressId { get; set; }

        [Required(ErrorMessage = "Street Address cannot be blank.")]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "Barangay cannot be blank.")]
        public string Barangay { get; set; }

        [Required(ErrorMessage = "City cannot be blank.")]
        public string City { get; set; }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }
    }
}
