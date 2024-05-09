namespace Uppgift14_Garage30.Models.ViewModels
{
    public class VehicleReceiptViewModel
    {
        public string RegistrationNumber { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public TimeSpan TotalParkingPeriod { get; set; }
        public decimal Price { get; set; }
        public string MemberName { get; set; }
    }
}
