using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Query;
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
            int maxAge = 95;
            int minAge = 18;
            const string errorMessage = "Your Personal Id is registered already";
            const string errorMessageFormatNumber = "Your Personal Id number must contain 12 numbers.";
            const string errorMessageFormatChar = "Your Personal Id number must contain just numbers and no characters";
            const string errorMessageLength = "Your Personal Id number must not exceed 12 digits";
            string errorMessageMinAge = $"Your age must be over {minAge.ToString()}";
            string errorMessageMaxAge = $"Your age must be under {maxAge.ToString()}";


            if (value is string input && !string.IsNullOrEmpty(input))
            {
                var existingPersonalId = context.Member.FirstOrDefault(i => i.PersonalId == input);
                if (existingPersonalId != null)
                {
                    return new ValidationResult(errorMessage);
                }
                // chech if contains characters
                if (input.Any(char.IsLetter))
                {
                    return new ValidationResult(errorMessageFormatChar);
                }
                // chech if lenght exceed 10 digits
                if (input.Length > 10)
                {
                    return new ValidationResult(errorMessageLength);
                }
                // chech if input contains only digits and has a length of 10
                if (input.Length != 10 || !input.All(char.IsDigit))
                {
                    return new ValidationResult(errorMessageFormatNumber);
                }

                // check if the first 2 digis are in the correct range
                int nowYear = DateTime.Now.Year;
                int userAge = 0;
                string nowYearFirstTwoDigits = DateTime.Now.Year.ToString().Substring(0, 2);
                string previousYearFirstTwoDigits = (nowYear - 100).ToString().Substring(0, 2);

                string firstTwoDigitsInput = input.Substring(0, 2);
                int yearTransformed = int.Parse(nowYearFirstTwoDigits + firstTwoDigitsInput);
                int previousYearTransformed = int.Parse(previousYearFirstTwoDigits + firstTwoDigitsInput);


                if (yearTransformed > nowYear)
                {
                    userAge  = previousYearTransformed;
                    if((nowYear - userAge) < minAge) 
                    {
                        return new ValidationResult(errorMessageMinAge);
                    }
                    if ((nowYear - userAge) > maxAge)
                    {
                        return new ValidationResult(errorMessageMaxAge);
                    }
                }
                if (yearTransformed < nowYear)
                {
                    userAge = yearTransformed;
                    if ((nowYear - userAge) < minAge)
                    {
                        return new ValidationResult(errorMessageMinAge);
                    }
                    if ((nowYear - userAge) > maxAge)
                    {
                        return new ValidationResult(errorMessageMaxAge);
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
