using System.ComponentModel.DataAnnotations;

namespace Uppgift14_Garage30.Models.ViewModels
{
    public class MemberDetailsViewModel
    {
        [Display(Name = "Personal Id Number")]
        public string PersonalId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Vehicles owned by member")]
        public IEnumerable<Vehicle> Vehicles { get; set; }
    }
}
