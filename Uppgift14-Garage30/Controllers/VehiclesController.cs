using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Uppgift14_Garage30.Data;
using Uppgift14_Garage30.Filters;
using Uppgift14_Garage30.Models;
using Uppgift14_Garage30.Models.ViewModels;

namespace Uppgift14_Garage30.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly Uppgift14_Garage30Context _context;

        public VehiclesController(Uppgift14_Garage30Context context)
        {
            _context = context;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            var uppgift14_Garage30Context = _context.Vehicle.Include(v => v.Member).Include(v => v.VehicleType);
            return View(await uppgift14_Garage30Context.ToListAsync());
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .Include(v => v.Member)
                .Include(v => v.VehicleType)
                .FirstOrDefaultAsync(m => m.RegistrationNumber == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            ViewData["MemberPersonalId"] = new SelectList(_context.Member, "PersonalId", "PersonalId");
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleType, "Id", "Name");
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelStateIsValid]
        public async Task<IActionResult> Create([Bind("RegistrationNumber,Make,Model,Color,VehicleTypeId,MemberPersonalId")] VehicleCreateViewModel vehicleVM)
        {
            Vehicle vehicle = await VehicleCreateVMToVehicle(vehicleVM);
            _context.Add(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            ViewData["MemberPersonalId"] = new SelectList(_context.Member, "PersonalId", "PersonalId", vehicle.MemberPersonalId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleType, "Id", "Id", vehicle.VehicleTypeId);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("RegistrationNumber,Make,Model,Color,VehicleTypeId,MemberPersonalId")] Vehicle vehicle)
        {
            if (id != vehicle.RegistrationNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.RegistrationNumber))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberPersonalId"] = new SelectList(_context.Member, "PersonalId", "PersonalId", vehicle.MemberPersonalId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleType, "Id", "Id", vehicle.VehicleTypeId);
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .Include(v => v.Member)
                .Include(v => v.VehicleType)
                .Include(v => v.CurrentParking)
                .FirstOrDefaultAsync(m => m.RegistrationNumber == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicle.Remove(vehicle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(string id)
        {
            return _context.Vehicle.Any(e => e.RegistrationNumber == id);
        }

        private async Task<Vehicle> VehicleCreateVMToVehicle(VehicleCreateViewModel vehicleVM)
        {
            VehicleType vehicleType = await _context.VehicleType.FirstOrDefaultAsync(vt => vt.Id == vehicleVM.VehicleTypeId);
            Member member = await _context.Member.FirstOrDefaultAsync(m => m.PersonalId == vehicleVM.MemberPersonalId);
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

        // ParkedVehiclesOverview

        public ActionResult ParkedVehiclesOverview(string vehicleType, string registrationNumber)
        {

            var query = _context.Vehicle.AsQueryable();

            if (!string.IsNullOrWhiteSpace(vehicleType))
            {
                query = query.Where(v => v.VehicleType.Name.Contains(vehicleType));
            }

            if (!string.IsNullOrWhiteSpace(registrationNumber))
            {
                query = query.Where(v => v.RegistrationNumber.Contains(registrationNumber));
            }


            var parkedVehicles = query
                  .Where(v => v.CurrentParking != null) // Ensure only vehicles with parking are selected
                  .Select(v => new ParkedVehicleViewModel
                  {
                      OwnerFullName = v.Member.FirstName + " " + v.Member.LastName,
                      PersonalId = v.MemberPersonalId,
                      NumberOfVehicles = v.Member.Vehicles.Count,
                      VehicleType = v.VehicleType.Name,
                      RegistrationNumber = v.RegistrationNumber,
                      ParkingTime = DateTime.Now - (v.CurrentParking != null ? v.CurrentParking.ParkingStarted : DateTime.Now) // Protects against null
                  })
                  .ToList();

            return View(parkedVehicles);



        }

        public ActionResult CheckOutVehicle(string registrationNumber)
        {
            var parking = _context.CurrentParking.Include(p => p.Vehicle)
                                                   .ThenInclude(v => v.Member)
                                                   .FirstOrDefault(p => p.RegistrationNumber == registrationNumber);

            if (parking == null)
            {
                ViewBag.ErrorMessage = "No parked vehicle found for the provided registration number.";
                return View("Error");
            }

            parking.ParkingEnded = DateTime.Now;

            var receiptViewModel = new VehicleReceiptViewModel
            {
                RegistrationNumber = registrationNumber,
                CheckInTime = parking.ParkingStarted,
                CheckOutTime = parking.ParkingEnded.Value,
                TotalParkingPeriod = parking.TotalParkingPeriod,
                Price = parking.ParkingPrice,
                MemberName = $"{parking.Vehicle.Member.FirstName} {parking.Vehicle.Member.LastName}"
            };

            // Remove the parking record or update it as needed
            // _context.CurrentParkings.Remove(parking);
            // Or just save changes if you're updating the record instead of deleting it
            _context.SaveChanges();

            // To make it printable, you'll likely return this data to a view designed for printing
            return View("Vehiclereceipt", receiptViewModel);
        }



    }
}
