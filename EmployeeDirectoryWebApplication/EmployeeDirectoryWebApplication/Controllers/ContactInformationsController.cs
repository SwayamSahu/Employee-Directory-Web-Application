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
using OfficeOpenXml;

namespace EmployeeDirectoryWebApplication.Controllers
{
    [Authorize]
    public class ContactInformationsController : Controller
    {
        private readonly EmployeeAppDbContext _context;

        public ContactInformationsController(EmployeeAppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index1()
        {
            // Include related Employee data
            var contactInformations = await _context.ContactInformations
                .Include(c => c.Employee) // Include related Employee data
                .ToListAsync();

            return View(contactInformations);
        }

        // GET: ContactInformations
        public async Task<IActionResult> Index()
        {
            var contactList = await _context.ContactInformations
     .Join(
         _context.EmployeeProfiles,
         contact => contact.EmployeeId,
         profile => profile.EmployeeId,
         (contact, profile) => new ContactViewModel
         {
             OfficeLocation = contact.OfficeLocation,
             Phone = contact.Phone,
             Email = contact.Email,
             socialmediaprofile = contact.SocialMediaProfiles,
             EmployeeName = profile.EmployeeName,
             contactId=contact.ContactId
             
         }
     )
     .ToListAsync();

            return View(contactList);
        }

        // GET: ContactInformations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactInformation = await _context.ContactInformations
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (contactInformation == null)
            {
                return NotFound();
            }

            return View(contactInformation);
        }

        [Authorize(Roles = "Admin")]
        // GET: ContactInformations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContactInformations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactInformation contactInformation)
        {
            if (ModelState.IsValid)
            {
                contactInformation.UpdatedDate = DateTime.Now;
                contactInformation.CreatedDate = DateTime.Now;
                contactInformation.CreatedBy=GetCurrentUserId();
                contactInformation.UpdatedBy=GetCurrentUserId();

                _context.Add(contactInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactInformation);
        }
        public string GetCurrentUserId()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userid;
        }

        [Authorize(Roles = "Admin")]
        // GET: ContactInformations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactInformation = await _context.ContactInformations.FindAsync(id);
            if (contactInformation == null)
            {
                return NotFound();
            }
            return View(contactInformation);
        }

        // POST: ContactInformations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  ContactInformation contactInformation)
        {
            if (id != contactInformation.ContactId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    contactInformation.UpdatedDate = DateTime.Now;
                    contactInformation.CreatedDate = contactInformation.CreatedDate;
                    contactInformation.CreatedBy = contactInformation.CreatedBy;
                    contactInformation.UpdatedBy = GetCurrentUserId();
                    _context.Update(contactInformation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactInformationExists(contactInformation.ContactId))
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
            return View(contactInformation);
        }

        [Authorize(Roles = "Admin")]
        // GET: ContactInformations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactInformation = await _context.ContactInformations
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (contactInformation == null)
            {
                return NotFound();
            }

            return View(contactInformation);
        }

        // POST: ContactInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contactInformation = await _context.ContactInformations.FindAsync(id);
            if (contactInformation != null)
            {
                _context.ContactInformations.Remove(contactInformation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactInformationExists(int id)
        {
            return _context.ContactInformations.Any(e => e.ContactId == id);
        }


        [HttpGet]
        public async Task<IActionResult> GenerateReport()
        {
            string userid=GetCurrentUserId();
            IQueryable<ContactInformation>ContactQuery=_context.ContactInformations.OrderBy(o=>o.ContactId);
            var contacts=await ContactQuery.ToListAsync();
            byte[] fileContents = GenerateReportAsExcel(contacts);
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Contacts.xlsx");
        }

        private byte[] GenerateReportAsExcel(List<ContactInformation> contactInformations)
        {
            using (var package=new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Contacts");
                var userid=GetCurrentUserId() ;
                var user=_context.Users.FirstOrDefault(x=>x.Id==userid);

                worksheet.Cells[1,1].Value = "Generator's Name: ";

                worksheet.Cells[2, 1].Value = "Generator's Email: ";

                worksheet.Cells[4, 1].Value = "Employee Name";

                worksheet.Cells[4, 2].Value = "Email";

                worksheet.Cells[4, 3].Value = "Phone";

                worksheet.Cells[4, 4].Value = "Office Location";

                worksheet.Cells[4, 5].Value = "Social Media Profile";

                worksheet.Cells[1, 2].Value = user.UserName;
                worksheet.Cells[2,2].Value = user.Email;

                int row = 5;
                foreach(var contact in contactInformations)
                {
                    var emp=_context.EmployeeProfiles.FirstOrDefault(x=>x.EmployeeId==contact.EmployeeId);
                    worksheet.Cells[row, 1].Value = emp.EmployeeName;
                    worksheet.Cells[row, 2].Value = contact.Email;
                    worksheet.Cells[row, 3].Value = contact.Phone;
                    worksheet.Cells[row, 4].Value = contact.OfficeLocation;
                    worksheet.Cells[row, 5].Value = contact.SocialMediaProfiles;
                    row++;

                }
                return package.GetAsByteArray();
            }
        }
    }
}
