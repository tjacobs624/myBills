using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BillManager.Data;
using BillManager.Models;

namespace BillManager.Pages.Billers
{
    public class CreateModel : PageModel
    {
        private readonly BillManager.Data.BillContext _context;

        public CreateModel(BillManager.Data.BillContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Biller Biller { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Billers.Add(Biller);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
