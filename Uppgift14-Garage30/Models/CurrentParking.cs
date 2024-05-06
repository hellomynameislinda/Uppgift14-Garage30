using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;

namespace Uppgift14_Garage30.Models
{
    public class CurrentParking
    {
        public string RegistrationNumber { get; set; }
        public DateTime ParkingStarted { get; set; } = DateTime.Now;

       

        // Navigational property one-to-one
        public Vehicle Vehicle { get; set; }
    }
}
