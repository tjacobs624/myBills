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
    public class IndexModel : PageModel
    {
        private readonly BillManager.Data.BillContext _context;

        public IndexModel(BillManager.Data.BillContext context)
        {
            _context = context;
        }

        public IList<Biller> Biller { get; set; } = default!;

        public async Task OnGetAsync()
        {
            // Ensure bills are loaded along with billers
            Biller = await _context.Billers
                .Include(b => b.Bills)  // Include related Bills to calculate total amount due
                .ToListAsync();
        }
    }
}
