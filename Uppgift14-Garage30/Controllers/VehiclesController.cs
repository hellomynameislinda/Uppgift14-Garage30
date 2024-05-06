using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Uppgift14_Garage30.Data;
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
        public async Task<IActionResult> Create([Bind("RegistrationNumber,Make,Model,Color,VehicleTypeId,MemberPersonalId")] VehicleCreateViewModel vehicleVM)
        {
            if (ModelState.IsValid)
            {
                // Even if this model is valid we need to check that the RegistrationNumber is unique
                if (await _context.Vehicle.AnyAsync(v => v.RegistrationNumber == vehicleVM.RegistrationNumber))
                {
                    // TODO: Add error message
                }
                else
                {
                    Vehicle vehicle = await vehicleVM.ToVehicle(_context);
                    _context.Add(vehicle);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["MemberPersonalId"] = new SelectList(_context.Member, "PersonalId", "PersonalId", vehicleVM.MemberPersonalId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleType, "Id", "Name", vehicleVM.VehicleTypeId);
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
    }
}
