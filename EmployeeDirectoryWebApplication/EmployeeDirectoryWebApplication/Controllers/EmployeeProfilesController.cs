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
    public class EmployeeProfilesController : Controller
    {
        private readonly EmployeeAppDbContext _context;

        public EmployeeProfilesController(EmployeeAppDbContext context)
        {
            _context = context;
        }

        // GET: EmployeeProfiles
        public async Task<IActionResult> Index()
        {
            var employeeAppDbContext = _context.EmployeeProfiles.Include(e => e.Dept).Include(e => e.Designation);
            return View(await employeeAppDbContext.ToListAsync());
        }

        // GET: EmployeeProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeProfile = await _context.EmployeeProfiles
                .Include(e => e.Dept)
                .Include(e => e.Designation)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employeeProfile == null)
            {
                return NotFound();
            }

            return View(employeeProfile);
        }

        [Authorize(Roles="Admin")]
        // GET: EmployeeProfiles/Create
        public IActionResult Create()
        {
            ViewData["DeptId"] = new SelectList(_context.Departments, "DeptId", "DeptId");
            ViewData["DesignationId"] = new SelectList(_context.Designations, "DesignationId", "DesignationId");
            return View();
        }

        // POST: EmployeeProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeProfile employeeProfile)
        {
            if (ModelState.IsValid)
            {

                employeeProfile.CreatedBy=GetCurrentUserId();
                employeeProfile.UpdatedBy=GetCurrentUserId();
                employeeProfile.UpdatedDate=DateTime.Now;
                employeeProfile.CreatedDate=DateTime.Now;
                _context.Add(employeeProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeptId"] = new SelectList(_context.Departments, "DeptId", "DeptId", employeeProfile.DeptId);
            ViewData["DesignationId"] = new SelectList(_context.Designations, "DesignationId", "DesignationId", employeeProfile.DesignationId);
            return View(employeeProfile);
        }
        public string GetCurrentUserId()
        {
            var userid= User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userid;
        }

        // GET: EmployeeProfiles/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeProfile = await _context.EmployeeProfiles.FindAsync(id);
            if (employeeProfile == null)
            {
                return NotFound();
            }
            ViewData["DeptId"] = new SelectList(_context.Departments, "DeptId", "DeptId", employeeProfile.DeptId);
            ViewData["DesignationId"] = new SelectList(_context.Designations, "DesignationId", "DesignationId", employeeProfile.DesignationId);
            return View(employeeProfile);
        }

        // POST: EmployeeProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeProfile employeeProfile)
        {
            if (id != employeeProfile.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    employeeProfile.CreatedBy = employeeProfile.CreatedBy;
                    employeeProfile.UpdatedBy = GetCurrentUserId();
                    employeeProfile.UpdatedDate = DateTime.Now;
                    employeeProfile.CreatedDate = employeeProfile.CreatedDate;
                    _context.Update(employeeProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeProfileExists(employeeProfile.EmployeeId))
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
            ViewData["DeptId"] = new SelectList(_context.Departments, "DeptId", "DeptId", employeeProfile.DeptId);
            ViewData["DesignationId"] = new SelectList(_context.Designations, "DesignationId", "DesignationId", employeeProfile.DesignationId);
            return View(employeeProfile);
        }

        // GET: EmployeeProfiles/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeProfile = await _context.EmployeeProfiles
                .Include(e => e.Dept)
                .Include(e => e.Designation)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employeeProfile == null)
            {
                return NotFound();
            }

            return View(employeeProfile);
        }

        // POST: EmployeeProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employeeProfile = await _context.EmployeeProfiles.FindAsync(id);
            if (employeeProfile != null)
            {
                _context.EmployeeProfiles.Remove(employeeProfile);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeProfileExists(int id)
        {
            return _context.EmployeeProfiles.Any(e => e.EmployeeId == id);
        }
    }
}
