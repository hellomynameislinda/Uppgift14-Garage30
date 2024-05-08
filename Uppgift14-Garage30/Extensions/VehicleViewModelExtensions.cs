using Microsoft.EntityFrameworkCore;
using Uppgift14_Garage30.Data;
using Uppgift14_Garage30.Models;
using Uppgift14_Garage30.Models.ViewModels;

namespace Uppgift14_Garage30.Extensions
{
    public static class VehicleViewModelExtensions
    {
        public static VehicleEditViewModel VehicleToVehicleEditVM(this Vehicle vehicle)
            => new()
            {
                RegistrationNumber = vehicle.RegistrationNumber,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Color = vehicle.Color,
                VehicleTypeId = vehicle.VehicleTypeId,
                MemberPersonalId = vehicle.MemberPersonalId,
                VehicleTypeName = vehicle.VehicleType.Name
            };

        public static async Task<Vehicle> VehicleEditVMToVehicle(this VehicleEditViewModel vehicleVM, Uppgift14_Garage30Context context)
        {
            VehicleType vehicleType = await context.VehicleType.FirstOrDefaultAsync(vt => vt.Id == vehicleVM.VehicleTypeId);
            Member member = await context.Member.FirstOrDefaultAsync(m => m.PersonalId == vehicleVM.MemberPersonalId);
            return new Vehicle()
            {
                RegistrationNumber = vehicleVM.RegistrationNumber,
                Make = vehicleVM.Make,
                Model = vehicleVM.Model,
                Color = vehicleVM.Color,
                VehicleTypeId = vehicleVM.VehicleTypeId,
                MemberPersonalId = vehicleVM.MemberPersonalId,
                VehicleType = vehicleType,
                Member = member
            };
        }
    }
}
