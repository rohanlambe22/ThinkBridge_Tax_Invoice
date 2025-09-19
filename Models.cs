using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BuggyApp.Data
{
    public class Invoice
    {
        [Key]
        public int InvoiceID { get; set; }

        [Required]
        [MaxLength(100)]
        public string CustomerName { get; set; }

        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
    }

    public class InvoiceItem
    {
        [Key]
        public int ItemID { get; set; }

        public int InvoiceID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public Invoice Invoice { get; set; }
    }
}


