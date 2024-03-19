using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InvoiceManager.Models.Domains
{
    public class InvoicePosition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Lp { get; set; }
        public int InvoiceId { get; set; }

        [Display(Name = "Wartość")]
        public decimal Value { get; set; }

        [Display(Name = "Produkt")]
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "Ilość")]
        public int Quantity { get; set; }


        public Invoice Invoice { get; set; }


        public Product Product { get; set; }
    }
}