namespace Uppgift14_Garage30.Models
{
    public class ParkedVehicleViewModel
    {
        public string OwnerFullName { get; set; }
        public string PersonalId { get; set; }
        public int NumberOfVehicles { get; set; }
        public string VehicleType { get; set; }
        public string RegistrationNumber { get; set; }
        public TimeSpan ParkingTime { get; set; }
    }
}
