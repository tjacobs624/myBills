using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillManager.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        public int BillId { get; set; }

        [ForeignKey("BillId")]
        public Bill Bill { get; set; } = default!;

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DatePaid { get; set; }

        public bool IsPartial { get; set; } = false;
    }
}
