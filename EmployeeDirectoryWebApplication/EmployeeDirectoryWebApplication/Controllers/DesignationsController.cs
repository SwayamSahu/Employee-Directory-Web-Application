using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeDirectoryWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EmployeeDirectoryWebApplication.Controllers
{
    [Authorize]
    public class DesignationsController : Controller
    {
        private readonly EmployeeAppDbContext _context;

        public DesignationsController(EmployeeAppDbContext context)
        {
            _context = context;
        }

        // GET: Designations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Designations.ToListAsync());
        }

        // GET: Designations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var designation = await _context.Designations
                .FirstOrDefaultAsync(m => m.DesignationId == id);
            if (designation == null)
            {
                return NotFound();
            }

            return View(designation);
        }
        [Authorize(Roles = "Admin")]
        // GET: Designations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Designations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Designation designation)
        {
            if (ModelState.IsValid)
            {
                designation.UpdatedDate = DateTime.Now;
                designation.CreatedDate = DateTime.Now;
                designation.CreatedBy = GetCurrentUserId();
                designation.UpdatedBy = GetCurrentUserId();
                _context.Add(designation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(designation);
        }

        public string GetCurrentUserId()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userid;
        }

        [Authorize(Roles = "Admin")]
        // GET: Designations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var designation = await _context.Designations.FindAsync(id);
            if (designation == null)
            {
                return NotFound();
            }
            return View(designation);
        }

        // POST: Designations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Designation designation)
        {
            if (id != designation.DesignationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    designation.UpdatedDate = DateTime.Now;
                    designation.CreatedDate = designation.CreatedDate;
                    designation.CreatedBy = designation.CreatedBy;
                    designation.UpdatedBy = GetCurrentUserId();
                    _context.Update(designation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DesignationExists(designation.DesignationId))
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
            return View(designation);
        }
        [Authorize(Roles = "Admin")]
        // GET: Designations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var designation = await _context.Designations
                .FirstOrDefaultAsync(m => m.DesignationId == id);
            if (designation == null)
            {
                return NotFound();
            }

            return View(designation);
        }

        // POST: Designations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var designation = await _context.Designations.FindAsync(id);
            if (designation != null)
            {
                _context.Designations.Remove(designation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DesignationExists(int id)
        {
            return _context.Designations.Any(e => e.DesignationId == id);
        }
    }
}
