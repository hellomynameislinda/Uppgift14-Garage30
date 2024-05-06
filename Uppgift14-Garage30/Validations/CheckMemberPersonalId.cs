using System.ComponentModel.DataAnnotations;
using Uppgift14_Garage30.Data;

namespace Uppgift14_Garage30.Validations
{
    public class CheckMemberPersonalId : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var context = validationContext.GetRequiredService<Uppgift14_Garage30Context>();

            const string errorMessage = "Your Personal Id is registered already";
            const string errorMessageFormatNumber = "Your Personal Id number must contain 10 numbers.";
            const string errorMessageFormatChar = "Your Personal Id number must contain just numbers and no characters";
            const string errorMessageLength = "Your Personal Id number must not exceed 10 digits";

            if (value is string input && !string.IsNullOrEmpty(input))
            {
                var existingPersonalId = context.Member.FirstOrDefault(i => i.PersonalId == input);
                if (existingPersonalId != null)
                {
                    return new ValidationResult(errorMessage);
                }
                if (input.Any(char.IsLetter))
                {
                    return new ValidationResult(errorMessageFormatChar);
                }
                if (input.Length > 10)
                {
                    return new ValidationResult(errorMessageLength);
                }
                if (input.Length != 10 || !input.All(char.IsDigit))
                {
                    return new ValidationResult(errorMessageFormatNumber);
                }

            }
            return ValidationResult.Success;
        }
    }
}
