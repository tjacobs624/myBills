using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BillManager.Data;
using BillManager.Models;

namespace BillManager.Pages.Billers
{
    public class EditModel : PageModel
    {
        private readonly BillManager.Data.BillContext _context;

        public EditModel(BillManager.Data.BillContext context)
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

            var biller =  await _context.Billers.FirstOrDefaultAsync(m => m.Id == id);
            if (biller == null)
            {
                return NotFound();
            }
            Biller = biller;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Biller).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillerExists(Biller.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BillerExists(int id)
        {
            return _context.Billers.Any(e => e.Id == id);
        }
    }
}
