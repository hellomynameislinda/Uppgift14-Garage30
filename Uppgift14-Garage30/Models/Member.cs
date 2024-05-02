using System.ComponentModel.DataAnnotations;

namespace Uppgift14_Garage30.Models
{
    public class Member
    {
        [Key]
        public string PersonalId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Navigational property

        // A list as it is on the many side of a one-to-many relationship
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
