using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Uppgift14_Garage30.Data;

namespace Uppgift14_Garage30.Services
{
    public class VehicleTypeService(Uppgift14_Garage30Context context) : IVehicleTypeService
    {
        private readonly Uppgift14_Garage30Context _context = context;

        public async Task<IEnumerable<SelectListItem>> GetVehicleTypesAsync()
        {
            return await _context.VehicleType.Select(
                vt => new SelectListItem
                {
                    Text = vt.Name.ToString(),
                    Value = vt.Id.ToString()
                }).ToListAsync();
        }
    }
}
