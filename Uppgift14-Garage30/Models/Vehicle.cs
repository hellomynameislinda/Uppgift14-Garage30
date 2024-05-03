using System.ComponentModel.DataAnnotations;

namespace Uppgift14_Garage30.Models
{
    public class Vehicle
    {
        [Key]
        public string RegistrationNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }

        // Foreign keys
        public int VehicleTypeId { get; set; }
        public string PersonalId { get; set; }

        // Navigational properties, the one side of one-to-one/one-to-many
        public VehicleType VehicleType { get; set; }
        public Member Member { get; set; }
        public CurrentParking? CurrentParking { get; set; }

    }
}
