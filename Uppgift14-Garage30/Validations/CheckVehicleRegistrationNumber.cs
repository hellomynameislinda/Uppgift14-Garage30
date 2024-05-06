using System.ComponentModel.DataAnnotations;
using Uppgift14_Garage30.Data;

namespace Uppgift14_Garage30.Validations
{
    public class CheckVehicleRegistrationNumber : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var context = validationContext.GetRequiredService<Uppgift14_Garage30Context>();
            const string errorMessage = "This registration number is registered already";
            const string errorMessageFormat = "The registration number should be 3 letters and 3 numbers";

            if (value is string input && !string.IsNullOrEmpty(input))
            {
                var existingRegistrationNumber = context.Vehicle.FirstOrDefault(v => v.RegistrationNumber == input);
                if (existingRegistrationNumber != null)
                {
                    return new ValidationResult(errorMessage);
                }

                string formatted = input.Replace(" ", "").Replace("-", "").ToUpperInvariant();
                bool validFormat = input.Length == 6 && input[..3].All(char.IsLetter) && input[3..].All(char.IsDigit);
                if (!validFormat)
                {
                    return new ValidationResult(errorMessageFormat);
                }
            }
            return ValidationResult.Success;
        }
    }
}
