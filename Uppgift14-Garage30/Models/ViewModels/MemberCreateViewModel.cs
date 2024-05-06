using System.ComponentModel.DataAnnotations;
using Uppgift14_Garage30.Validations;
namespace Uppgift14_Garage30.Models.ViewModels

{
    public class MemberCreateViewModel
    {
        [RegularExpression(@"^[^-\s]+$", ErrorMessage = "Personal Id Number cannot contain spaces or negative values.")]
        [StringLength(12, ErrorMessage = "Personal Id Number must be 12 characters long")]
        [Display(Name = "Personal Id Number")]

        [CheckMemberPersonalId]
        public string PersonalId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [CheckLastName]
        public string LastName { get; set; }


        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

    }
}
