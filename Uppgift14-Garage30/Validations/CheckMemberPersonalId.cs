using System.ComponentModel.DataAnnotations;
using Uppgift14_Garage30.Data;
using Uppgift14_Garage30.Models;

namespace Uppgift14_Garage30.Validations
{
    public class CheckMemberPersonalId : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var context = validationContext.GetRequiredService<Uppgift14_Garage30Context>();
            const string errorMessage = "Your Personal Id is registered already";
            const string errorMessageFormat = "Your Personal Id number must contain 10 numbers";

            if (value is string input && !string.IsNullOrEmpty(input))
            {
                var existingPersonalId = context.Member.FirstOrDefault(i => i.PersonalId == input);
                if (existingPersonalId != null)
                {
                    return new ValidationResult(errorMessage);
                }
                if(input.Length != 10 && input.All(char.IsDigit))
                {
                    return new ValidationResult(errorMessageFormat);
                }
            }
            return ValidationResult.Success;
        }
    }
}
