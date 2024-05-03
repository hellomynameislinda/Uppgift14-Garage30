using System.ComponentModel.DataAnnotations;
using Uppgift14_Garage30.Models;

namespace Uppgift14_Garage30.Validations
{
    public class CheckLastName : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            const string errorMessageSame = "Last name and first name can't be the same";
            const string errorMessageFormat = "Last Name must contain just characters";
            if (value is string input)
            {
                // check is last name is the same as first name
                if (validationContext.ObjectInstance is MemberCreateViewModel model && model.FirstName == input)
                {
                    return new ValidationResult(errorMessageSame);
                }
                //check if last name contais just characters
                if(!IsAlphabetical(input))
                {
                    return new ValidationResult(errorMessageFormat);
                }
                return ValidationResult.Success;
            }
            return new ValidationResult("Something went wrong!");  
        }

        private bool IsAlphabetical(string input)
        {
            foreach(char c in input)
            {
                if (!char.IsLetter(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
