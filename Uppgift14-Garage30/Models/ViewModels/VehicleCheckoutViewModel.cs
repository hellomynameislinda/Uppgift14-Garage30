using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Uppgift14_Garage30.Data;
using Uppgift14_Garage30.Validations;

namespace Uppgift14_Garage30.Models.ViewModels
{
    public class VehicleCheckoutViewModel
    {
        [Display(Name = "Registration Number")]
        public virtual string RegistrationNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }

        // Foreign keys
        [Display(Name = "Vehicle Type")]
        public int VehicleTypeId { get; set; }
        [Display(Name = "Member Personal ID Number")]
        public string MemberPersonalId { get; set; }

        // Additional properties
        [Display(Name = "Vehicle Type")]
        [ValidateNever]
        public string? VehicleTypeName { get; set; } = null;

        public CurrentParking? CurrentParking { get; set; }
    }
}
