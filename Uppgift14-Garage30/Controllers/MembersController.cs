using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using Uppgift14_Garage30.Data;
using Uppgift14_Garage30.Filters;
using Uppgift14_Garage30.Models;
using Uppgift14_Garage30.Models.ViewModels;

namespace Uppgift14_Garage30.Controllers
{
    public class MembersController : Controller
    {
        private readonly Uppgift14_Garage30Context _context;
        private Member? _currentMember;

        public MembersController(Uppgift14_Garage30Context context)
        {
            _context = context;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            return View(await _context.Member.ToListAsync());
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details()
        {
            string? id = HttpContext.Session.GetString("id");

            if (id == null)
            {
                if (_currentMember is not null)
                { 
                    id = _currentMember.PersonalId;
                }
                else
                { 
                    return NotFound();
                }
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.PersonalId == id);
            
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelStateIsValid]
        public async Task<IActionResult> Create(MemberCreateViewModel viewModel)
        {
            var member = new Member
            {
                PersonalId = viewModel.PersonalId,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
            };
            _context.Member.Add(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PersonalId,FirstName,LastName")] Member member)
        {
            if (id != member.PersonalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.PersonalId))
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
            return View(member);
        }

        // GET: Login Members
        public async Task<IActionResult> Login()
        {
            ViewBag.Members = new SelectList(_context.Member, nameof(Member.PersonalId), nameof(Member.PersonalId));
            return View();
        }

        // POST: Login Members
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(MemberLoginViewModel viewModel)
        {
            var PersonalId = viewModel.PersonalId;

            if (PersonalId == null)
            {
                return NoContent();  // Go back to the original login
            }

            var member = await _context.Member.FindAsync(PersonalId);
            if (member == null)
            {
                return NotFound();
            }

            // Sätt till property
            _currentMember = member;
            //HttpContext.Session.SetString("CurrentMemberId", PersonalId);
            //return RedirectToAction(nameof(Details), new { id = PersonalId });
            return RedirectToAction(nameof(Details));
        }


        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.PersonalId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var member = await _context.Member.FindAsync(id);
            if (member != null)
            {
                _context.Member.Remove(member);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(string id)
        {
            return _context.Member.Any(e => e.PersonalId == id);
        }
    }
}
