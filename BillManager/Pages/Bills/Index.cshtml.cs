using Microsoft.AspNetCore.Mvc.RazorPages;
using BillManager.Models;
using BillManager.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
    }
}
