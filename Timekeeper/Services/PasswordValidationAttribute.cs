using System.ComponentModel.DataAnnotations;

namespace Timekeeper.Services
{
    public class PasswordValidationAttribute : ValidationAttribute
    {
        public bool Skip { get; set; } = false;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (Skip)
            {
                return ValidationResult.Success!;
            }

            if (value is string password)
            {
                if (Password.ValidatePassword(password))
                {
                    return ValidationResult.Success!;
                }
                else
                {
                    return new ValidationResult("Password must be at least 8 characters long.");
                }
            }
            return new ValidationResult("Invalid password format.");
        }
    }
}
