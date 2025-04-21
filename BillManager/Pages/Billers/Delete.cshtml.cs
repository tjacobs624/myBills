using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BillManager.Data;
using BillManager.Models;

namespace BillManager.Pages.Billers
{
    public class DeleteModel : PageModel
    {
        private readonly BillManager.Data.BillContext _context;

        public DeleteModel(BillManager.Data.BillContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Biller Biller { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var biller = await _context.Billers.FirstOrDefaultAsync(m => m.Id == id);

            if (biller is not null)
            {
                Biller = biller;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var biller = await _context.Billers.FindAsync(id);
            if (biller != null)
            {
                Biller = biller;
                _context.Billers.Remove(Biller);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
