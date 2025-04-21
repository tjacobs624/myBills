// Models/Bill.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace BillManager.Models
{
    public class Bill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Biller Biller { get; set; } // Reference to the Biller class
        public int BillerId { get; set; } // Foreign key for the Biller
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsPaid { get; set; }
        public string Category { get; set; } // Optional category for the bill
        public DateTime? StopDate { get; set; } // Nullable stop date

        // Recurrence properties
        public RecurrenceType RecurrenceType { get; set; } // e.g., "Monthly", "Yearly"
        public int? RecurrenceInterval { get; set; } // e.g., every 1 month or every 2 years
        public DateTime? NextDueDate { get; set; } // Nullable to allow for one-time bills

        // Collection of payments
        public List<Payment> Payments { get; set; } = new List<Payment>();

        public bool IsFullyPaid => Payments.Sum(p => p.Amount) >= Amount;
    }


    public enum RecurrenceType
    {
        None,
        Daily,
        Weekly,
        Biweekly,
        Monthly,
        Yearly
    }
}
