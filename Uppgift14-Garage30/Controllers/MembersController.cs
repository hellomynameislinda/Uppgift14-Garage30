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
        private readonly IHttpContextAccessor _httpContextAccessor;

        //private Member? _currentMember;

        public MembersController(Uppgift14_Garage30Context context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            return View(await _context.Member.ToListAsync());
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(string? id = null)
        {
            // If we want the Details function without parameter we would use the line below to get
            // the id, but defaulting it to zero if not present should be more backwards compatible
            //string? id = HttpContext.Request.RouteValues["id"]?.ToString();

            var session = _httpContextAccessor.HttpContext.Session; // Added to use session for login
            
            if (id == null)
            {
                if (session.GetString("CurrentUserId") is not null)
                { 
                    id = session.GetString("CurrentUserId");  // Added to use session for login
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
            var members = await _context.Member
                .Select(m => new
                {
                    PersonalId = m.PersonalId,
                    DisplayText = $"{m.PersonalId} - {m.LastName}, {m.FirstName}"
                })
                .ToListAsync();
            ViewBag.Members = new SelectList(members, nameof(Member.PersonalId), "DisplayText");

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

            var session = _httpContextAccessor.HttpContext.Session; // Added to use session for login
            session.SetString("CurrentUserId", PersonalId); // Added to use session for login

            // Sätt till property
            //_currentMember = member;
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
