using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Uppgift14_Garage30.Data;
using Uppgift14_Garage30.Validations;

namespace Uppgift14_Garage30.Models.ViewModels
{
    public class VehicleCreateViewModel
    {
        [Display(Name = "Registration Number")]
        [CheckVehicleRegistrationNumber]
        public string RegistrationNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }

        // Foreign keys
        [Display(Name = "Vehicle Type")]
        public int VehicleTypeId { get; set; }
        [Display(Name = "Member Personal ID Number")]
        public string MemberPersonalId { get; set; }

        public async Task<Vehicle> ToVehicle(Uppgift14_Garage30Context context)
        {
            VehicleType vehicleType = await context.VehicleType.FirstOrDefaultAsync(vt => vt.Id == VehicleTypeId);
            Member member = await context.Member.FirstOrDefaultAsync(m => m.PersonalId == MemberPersonalId);
            return new Vehicle()
            {
                RegistrationNumber = RegistrationNumber,
                Make = Make,
                Model = Model,
                Color = Color,
                VehicleTypeId = VehicleTypeId,
                MemberPersonalId = MemberPersonalId,
                VehicleType = vehicleType,
                Member = member
            };
        }
    }
}
