using System.Collections.Generic;

namespace BillManager.Models
{
    public class Biller
    {
        public int Id { get; set; }  // Unique identifier for the biller

        public string Name { get; set; } = string.Empty;  // Name of the biller (e.g., utility company)

        public string ContactInfo { get; set; } = string.Empty;  // Contact information (email, phone, etc.)

        public string PaymentMethods { get; set; } = string.Empty;  // Possible payment methods (e.g., Credit Card, Bank Transfer, etc.)

        // Navigation property to related bills
        public ICollection<Bill> Bills { get; set; } = new List<Bill>();
    }

    public class BillerWithAmountDue
    {
        public Biller Biller { get; set; } = null!;  // Reference to the Biller class

        public decimal AmountDue { get; set; }  // Amount due for the biller
    }
}
