﻿namespace Uppgift14_Garage30.Models
{
    public class VehicleType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigational property
        public ICollection<Vehicle> Vehicles { get; set; }

    }
}
