using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Uppgift14_Garage30.Data;


namespace Uppgift14_Garage30.Validations
{
    public class CheckMemberPersonalId : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var context = validationContext.GetRequiredService<Uppgift14_Garage30Context>();
            int maxAge = 95;
            int minAge = 18;
            const string errorMessage = "Your Personal Id is registered already";
            const string errorMessageFormatNumber = "Your Personal Id number must contain 12 numbers.";
            const string errorMessageFormatChar = "Your Personal Id number must contain just numbers and no characters";
            const string errorMessageLength = "Your Personal Id number must not exceed 12 digits";
            const string errorMessageDate = "Could not extract birthdate from your Personal Id";
            string errorMessageMinAge = $"Your age must be over {minAge.ToString()}";
            string errorMessageMaxAge = $"Your age must be under {maxAge.ToString()}";


            if (value is string input && !string.IsNullOrEmpty(input))
            {
                var existingPersonalId = context.Member.FirstOrDefault(i => i.PersonalId == input);
                if (existingPersonalId != null)
                {
                    return new ValidationResult(errorMessage);
                }
                // Chech if contains characters
                if (input.Any(char.IsLetter))
                {
                    return new ValidationResult(errorMessageFormatChar);
                }
                // Chech if lenght exceed 12 digits
                if (input.Length != 12)
                {
                    return new ValidationResult(errorMessageLength);
                }
                // Chech if input contains only digits
                if (!input.All(char.IsDigit))
                {
                    return new ValidationResult(errorMessageFormatNumber);
                }

                // Calculate age
                string personalNrDate = input.Substring(0, 8);
                DateTime birthDate;

                if (!DateTime.TryParseExact(personalNrDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthDate))
                {
                    //Check after ambigous if parsing fails
                    if(DateTime.TryParseExact(personalNrDate, new[] {"MMddyy", "ddMMyy"}, CultureInfo.InvariantCulture, DateTimeStyles.None, out birthDate))
                    {
                        // if success continue with calculate age
                    }
                    else
                    {
                        return new ValidationResult(errorMessageDate);
                    }
                }

                //Calculate age in years
                DateTime currentDate = DateTime.Now.Date;
                int ageYears = currentDate.Year - birthDate.Year;

                //Check if the birthday passed current year and adjust
                if(birthDate > currentDate.AddDays(-ageYears))
                {
                    ageYears--;
                }

                //Check if age is within the valid range
                if(ageYears < minAge)
                {
                    return new ValidationResult(errorMessageMinAge);
                }
                if (ageYears > maxAge)
                {
                    return new ValidationResult(errorMessageMaxAge);
                }
            }
            return ValidationResult.Success;
        }
    }
}

