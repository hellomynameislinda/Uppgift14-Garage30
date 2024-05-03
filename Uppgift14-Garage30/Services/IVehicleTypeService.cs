using Microsoft.AspNetCore.Mvc.Rendering;

namespace Uppgift14_Garage30.Services
{
    public interface IVehicleTypeService
    {
        Task<IEnumerable<SelectListItem>> GetVehicleTypesAsync();
    }
}