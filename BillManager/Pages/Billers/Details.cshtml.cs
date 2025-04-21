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
    public class DetailsModel : PageModel
    {
        private readonly BillManager.Data.BillContext _context;

        public DetailsModel(BillManager.Data.BillContext context)
        {
            _context = context;
        }

        // Mark as nullable
        public Biller? Biller { get; set; } = null;

        // Change to ICollection
        public ICollection<Bill> Bills { get; set; } = new List<Bill>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Fetch the biller along with its related bills
            Biller = await _context.Billers
                .Include(b => b.Bills) // Load related bills
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Biller == null)
            {
                return NotFound();
            }

            // Populate the list of bills for this biller
            Bills = Biller.Bills;

            return Page();
        }
    }
}
