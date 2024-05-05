using Microsoft.EntityFrameworkCore;
using Uppgift14_Garage30.Data;

namespace Uppgift14_Garage30.Models.ViewModels
{
    public class VehicleCreateViewModel
    {
        public string RegistrationNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }

        // Foreign keys
        public int VehicleTypeId { get; set; }
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
