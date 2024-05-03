using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Uppgift14_Garage30.Filters
{
    public class ModelStateIsValid : ActionFilterAttribute //Attribute, IActionFilter
    {       

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ViewResult
                {
                    StatusCode = 400,
                    TempData = ((Controller)context.Controller).TempData,
                    ViewData = ((Controller)context.Controller).ViewData
                };
            }
        }
    }
}
