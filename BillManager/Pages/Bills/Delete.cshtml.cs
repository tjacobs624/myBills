using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BillManager.Models;
using BillManager.Data;

namespace BillManager.Pages.Bills
{
    public class DeleteModel : PageModel
    {
        private readonly BillContext _context;

        public DeleteModel(BillContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Bill Bill { get; set; }

        public IActionResult OnGet(int id)
        {
            Bill = _context.Bills.FirstOrDefault(b => b.Id == id);
            if (Bill == null)
                return NotFound();

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var bill = _context.Bills.FirstOrDefault(b => b.Id == id);
            if (bill == null)
                return NotFound();

            _context.Bills.Remove(bill);
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
}
