using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BillManager.Models;
using BillManager.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace BillManager.Pages.Bills
{
    public class EditModel : PageModel
    {
        private readonly BillContext _context;

        public EditModel(BillContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BillInput Input { get; set; }

        public int Id { get; set; }
        public List<SelectListItem> Billers { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            var bill = _context.Bills.FirstOrDefault(b => b.Id == id);
            if (bill == null)
            {
                return NotFound();
            }

            Input = new BillInput
            {
                Name = bill.Name,
                Amount = bill.Amount,
                DueDate = bill.DueDate,
                IsPaid = bill.IsPaid,
                Category = bill.Category,
                RecurrenceType = bill.RecurrenceType,
                StopDate = bill.StopDate,
                BillerId = bill.BillerId,
                FilePath = bill.FilePath // Ensure FilePath is populated
            };

            LoadBillers(); // Populate the Billers dropdown
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate Billers list in case of validation failure
                LoadBillers();
                return Page();
            }

            var billToUpdate = _context.Bills.FirstOrDefault(b => b.Id == id);
            if (billToUpdate == null)
                return NotFound();

            billToUpdate.Name = Input.Name;
            billToUpdate.Amount = Input.Amount;
            billToUpdate.DueDate = Input.DueDate;
            billToUpdate.IsPaid = Input.IsPaid;
            billToUpdate.RecurrenceType = Input.RecurrenceType;
            billToUpdate.Category = Input.Category; // Update category
            billToUpdate.RecurrenceInterval = Input.RecurrenceInterval; // Update recurrence interval
            billToUpdate.StopDate = Input.StopDate;
            billToUpdate.BillerId = Input.BillerId; // Update biller

            _context.SaveChanges();

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostAsync(IFormFile File)
        {
            if (!ModelState.IsValid)
            {
                LoadBillers(); // Repopulate Billers list in case of validation failure
                return Page();
            }

            var bill = await _context.Bills.FindAsync(Input.Id);
            if (bill == null)
            {
                return NotFound();
            }

            // Handle file removal
            if (Input.RemoveFile && !string.IsNullOrEmpty(bill.FilePath) && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", bill.FilePath.TrimStart('/'))))
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", bill.FilePath.TrimStart('/'));
                System.IO.File.Delete(fullPath);
                bill.FilePath = null;
            }

            // Handle file upload
            if (File != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists
                var filePath = Path.Combine(uploadsFolder, File.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await File.CopyToAsync(stream);
                }

                bill.FilePath = $"/uploads/{File.FileName}";
            }

            // Update other fields
            bill.Name = Input.Name;
            bill.Amount = Input.Amount;
            bill.DueDate = Input.DueDate;
            bill.IsPaid = Input.IsPaid;
            bill.Category = Input.Category;
            bill.RecurrenceType = Input.RecurrenceType;
            bill.StopDate = Input.StopDate;
            bill.BillerId = Input.BillerId;

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostUpdateAsync(IFormFile File)
        {
            if (!ModelState.IsValid)
            {
                LoadBillers(); // Repopulate Billers list in case of validation failure
                return Page();
            }

            var bill = await _context.Bills.FindAsync(Input.Id);
            if (bill == null)
            {
                return NotFound();
            }

            // Handle file removal
            if (Input.RemoveFile && !string.IsNullOrEmpty(bill.FilePath) && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", bill.FilePath.TrimStart('/'))))
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", bill.FilePath.TrimStart('/'));
                System.IO.File.Delete(fullPath);
                bill.FilePath = null;
            }

            // Handle file upload
            if (File != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists
                var filePath = Path.Combine(uploadsFolder, File.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await File.CopyToAsync(stream);
                }

                bill.FilePath = $"/uploads/{File.FileName}";
            }

            // Update other fields
            bill.Name = Input.Name;
            bill.Amount = Input.Amount;
            bill.DueDate = Input.DueDate;
            bill.IsPaid = Input.IsPaid;
            bill.Category = Input.Category;
            bill.RecurrenceType = Input.RecurrenceType;
            bill.StopDate = Input.StopDate;
            bill.BillerId = Input.BillerId;

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private void LoadBillers()
        {
            Billers = _context.Billers
                .Distinct()
                .OrderBy(b => b.Name)
                .Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.Name
                })
                .ToList();
        }

        public class BillInput
        {
            public int Id { get; set; } // Added Id property

            public string Name { get; set; }
            public decimal Amount { get; set; }
            public DateTime DueDate { get; set; }
            public bool IsPaid { get; set; }
            public string Category { get; set; }
            public int RecurrenceInterval { get; set; } // Added RecurrenceInterval property
            public RecurrenceType RecurrenceType { get; set; }
            public DateTime? StopDate { get; set; }
            public int BillerId { get; set; }
            public string FilePath { get; set; } // Ensure this property exists
            public bool RemoveFile { get; set; } // For file removal
        }
    }
}