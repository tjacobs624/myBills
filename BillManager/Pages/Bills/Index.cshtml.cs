using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BillManager.Models;
using BillManager.Data;
using System.Collections.Generic;
using System.Linq;

namespace BillManager.Pages.Bills
{
    public class IndexModel : PageModel
    {
        private readonly BillContext _context;

        public IndexModel(BillContext context)
        {
            _context = context;
        }

        public List<Bill> Bills { get; set; }
        public string CurrentSort { get; set; }
        public int? CurrentBillerId { get; set; }

        public async Task OnGetAsync(string sortOrder, int? billerId)
        {
            CurrentSort = sortOrder;
            CurrentBillerId = billerId;

            IQueryable<Bill> bills = _context.Bills.Include(b => b.Biller);

            if (billerId.HasValue)
            {
                bills = bills.Where(b => b.BillerId == billerId.Value);
            }

            // Sorting logic
            bills = sortOrder switch
            {
                "biller_desc" => bills.OrderByDescending(b => b.Biller.Name),
                "dueDate" => bills.OrderBy(b => b.DueDate),
                "dueDate_desc" => bills.OrderByDescending(b => b.DueDate),
                _ => bills.OrderBy(b => b.Biller.Name)
            };

            Bills = await bills.ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteSelectedAsync(List<int> selectedBillIds)
        {
            if (selectedBillIds == null || !selectedBillIds.Any())
            {
                ModelState.AddModelError(string.Empty, "No bills selected for deletion.");
                Bills = await _context.Bills.Include(b => b.Biller).ToListAsync();
                return Page();
            }

            var billsToDelete = await _context.Bills
                .Where(b => selectedBillIds.Contains(b.Id))
                .ToListAsync();

            foreach (var bill in billsToDelete)
            {
                if (!string.IsNullOrEmpty(bill.FilePath) && System.IO.File.Exists(bill.FilePath))
                {
                    // Delete the file from the file system
                    System.IO.File.Delete(bill.FilePath);
                }
            }

            _context.Bills.RemoveRange(billsToDelete);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public IActionResult OnGetDownload(int id)
        {
            var bill = _context.Bills.FirstOrDefault(b => b.Id == id);
            if (bill == null || string.IsNullOrEmpty(bill.FilePath) || !System.IO.File.Exists(bill.FilePath))
            {
                return NotFound();
            }

            var fileName = Path.GetFileName(bill.FilePath);
            var fileBytes = System.IO.File.ReadAllBytes(bill.FilePath);
            return File(fileBytes, "application/octet-stream", fileName);
        }
    }
}
