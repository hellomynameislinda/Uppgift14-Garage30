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
      

        
   
       
        [Display(Name = "Member Personal ID Number")]
        public string MemberPersonalId { get; set; }

      
    

        public CurrentParking? CurrentParking { get; set; }
    }
}
