using System.ComponentModel.DataAnnotations;
using Uppgift14_Garage30.Validations;

namespace Uppgift14_Garage30.Models.ViewModels
{
    public class MemberEditViewModel
    {
        public string PersonalId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [CheckLastName]
        public string LastName { get; set; }


        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
