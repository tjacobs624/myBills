using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BillManager.Data;
using BillManager.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BillManager.Pages.Bills
{
    public class CreateModel : PageModel
    {
        private readonly BillContext _context;

        public CreateModel(BillContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BillInput Input { get; set; }

        public List<SelectListItem> Billers { get; set; } = new();

        // OnGet method to fetch the billers from the database
        public IActionResult OnGet()
        {
            // Populate Billers list
            LoadBillers();
            return Page();
        }

        // OnPost method to handle form submission
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // Repopulate Billers list in case of validation failure
                LoadBillers();
                return Page();
            }

            // Create a new bill object and set its properties
            var bill = new Bill
            {
                Name = Input.Name,
                Amount = Input.Amount,
                DueDate = Input.DueDate,
                IsPaid = Input.IsPaid,
                RecurrenceType = Input.RecurrenceType,
                Category = Input.Category,
                RecurrenceInterval = Input.RecurrenceInterval,
                StopDate = Input.StopDate,
                BillerId = Input.BillerId // Set the selected biller
            };

            // Add the initial bill to the database
            _context.Bills.Add(bill);
            
            // Generate recurring bills if needed
            if (bill.StopDate.HasValue)
            {
                GenerateRecurringBills(bill);
            }

            _context.SaveChanges();

            // Redirect to the Bills Index page after successful creation
            return RedirectToPage("./Index");
        }

        // Helper method to populate Billers list
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

        // Recurrence logic to generate recurring bills
        private void GenerateRecurringBills(Bill bill)
        {
            DateTime nextDueDate = bill.DueDate;

            while (nextDueDate < bill.StopDate.Value)
            {
                Bill recurringBill = new Bill
                {
                    Name = bill.Name,
                    Amount = bill.Amount,
                    DueDate = nextDueDate,
                    IsPaid = bill.IsPaid,
                    RecurrenceType = bill.RecurrenceType,
                    Category = bill.Category,
                    RecurrenceInterval = bill.RecurrenceInterval,
                    BillerId = bill.BillerId
                };

                // Calculate next due date based on RecurrenceType
                switch (bill.RecurrenceType)
                {
                    case RecurrenceType.Weekly:
                        nextDueDate = nextDueDate.AddDays(7);
                        break;
                    case RecurrenceType.Biweekly:
                        nextDueDate = nextDueDate.AddDays(14);
                        break;
                    case RecurrenceType.Monthly:
                        nextDueDate = nextDueDate.AddMonths(1);
                        break;
                }

                _context.Bills.Add(recurringBill);
            }
        }

        // Class to bind the input fields from the form
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
            public RecurrenceType RecurrenceType { get; set; } // Weekly, Biweekly, Monthly
            
            public string? Category { get; set; }
            public int? RecurrenceInterval { get; set; } // For weekly, biweekly, or monthly intervals (if needed)
            
            [DataType(DataType.Date)]
            public DateTime? StopDate { get; set; } // Nullable stop date

            [Required]
            public int BillerId { get; set; } // Foreign key for the Biller
        }
    }
}
