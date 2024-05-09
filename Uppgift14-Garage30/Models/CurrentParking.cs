using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;

namespace Uppgift14_Garage30.Models
{
    public class CurrentParking
    {
        public string RegistrationNumber { get; set; }
        public DateTime ParkingStarted { get; set; } = DateTime.Now;
        public DateTime? ParkingEnded { get; set; } // Nullable to allow for currently parked vehicles

        // Navigational property one-to-one
        public Vehicle Vehicle { get; set; }

        // Calculate the total parking period
        public TimeSpan TotalParkingPeriod => (ParkingEnded ?? DateTime.Now) - ParkingStarted;

        // Calculate price based on a fixed hourly rate, for example, $5 per hour.
        public decimal ParkingPrice => (decimal)TotalParkingPeriod.TotalHours * 5; // Adjust the price per hour as needed
    }
}
