using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Uppgift14_Garage30.Data;
using Uppgift14_Garage30.Validations;

namespace Uppgift14_Garage30.Models.ViewModels
{
    public class VehicleCreateViewModel : VehicleEditViewModel
    {
        [CheckVehicleRegistrationNumber]
        public override string RegistrationNumber { get; set; }
    }
}
