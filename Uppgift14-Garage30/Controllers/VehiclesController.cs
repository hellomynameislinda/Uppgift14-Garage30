﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Uppgift14_Garage30.Data;
using Uppgift14_Garage30.Extensions;
using Uppgift14_Garage30.Filters;
using Uppgift14_Garage30.Models;
using Uppgift14_Garage30.Models.ViewModels;

namespace Uppgift14_Garage30.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly Uppgift14_Garage30Context _context;
        private readonly IIncludableQueryable<Vehicle, VehicleType> _vehicles;

        public VehiclesController(Uppgift14_Garage30Context context)
        {
            _context = context;
            _vehicles = _context.Vehicle.Include(v => v.VehicleType);
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            var vehicleVMs = _vehicles.Include(v => v.Member)
                .Select(v => v.VehicleToVehicleEditVM());
            return View(await vehicleVMs.ToListAsync());
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _vehicles.Include(v => v.Member)
                .FirstOrDefaultAsync(m => m.RegistrationNumber == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle.VehicleToVehicleEditVM());
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
        public async Task<IActionResult> Create([Bind("RegistrationNumber,Make,Model,Color,VehicleTypeId,MemberPersonalId")] VehicleCreateViewModel vehicleVM)
        {

            if (ModelState.IsValid)
            {
                Vehicle vehicle = await vehicleVM.VehicleEditVMToVehicle(_context);
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["MemberPersonalId"] = new SelectList(_context.Member, "PersonalId", "PersonalId", vehicleVM.MemberPersonalId);
            ViewData["VehicleTypeId"] = new SelectList(_context.Set<VehicleType>(), "Id", "Id", vehicleVM.VehicleTypeId);
            return View(vehicleVM);
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
            var viewModel = new VehicleEditViewModel
            {
                RegistrationNumber = vehicle.RegistrationNumber,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Color = vehicle.Color,
                VehicleTypeId = vehicle.VehicleTypeId,
                MemberPersonalId = vehicle.MemberPersonalId,
            };
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleType, "Id", "Name", vehicle.VehicleTypeId);
            return View(viewModel);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelStateIsValid]
        public async Task<IActionResult> Edit(string id, [Bind("RegistrationNumber,Make,Model,Color,VehicleTypeId,MemberPersonalId")] VehicleEditViewModel vehicleVM)
        {
            if (id != vehicleVM.RegistrationNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Vehicle vehicle = await vehicleVM.VehicleEditVMToVehicle(_context);
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicleVM.RegistrationNumber))
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
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleType, "Id", "Name", vehicleVM.VehicleTypeId);
            return View(vehicleVM);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _vehicles
                .Include(v => v.Member)
                .FirstOrDefaultAsync(m => m.RegistrationNumber == id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle.VehicleToVehicleEditVM());
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

    }
}
