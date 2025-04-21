using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BillManager.Models;
using BillManager.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
                return NotFound();

            Id = bill.Id;
            Input = new BillInput
            {
                Name = bill.Name,
                Amount = bill.Amount,
                DueDate = bill.DueDate,
                IsPaid = bill.IsPaid,
                RecurrenceType = bill.RecurrenceType,
                Category = bill.Category, // Add this if Category is part of your Bill model
                RecurrenceInterval = bill.RecurrenceInterval, // Add this if RecurrenceInterval is part of your Bill model
                StopDate = bill.StopDate,
                BillerId = bill.BillerId
            };

            // Populate Billers list
            LoadBillers();
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
            [Required]
            public string Name { get; set; }

            [Required]
            [Range(0.01, 10000)]
            public decimal Amount { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateTime DueDate { get; set; }

            public bool IsPaid { get; set; }

            [Required]
            public RecurrenceType RecurrenceType { get; set; }

            public string? Category { get; set; }
            public int? RecurrenceInterval { get; set; }

            [DataType(DataType.Date)]
            public DateTime? StopDate { get; set; }

            [Required]
            public int BillerId { get; set; }
        }
    }
}