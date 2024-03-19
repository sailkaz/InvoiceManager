using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Metadata.Edm;
using System.ComponentModel;

namespace InvoiceManager.Models.Domains
{
    public class Product
    {
        public Product()
        {
            InvoicePositions = new Collection<InvoicePosition>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [DisplayName("Nazwa produktu")]
        public string  Name { get; set; }

        [DisplayName("Cena produktu")]
        public decimal Price { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }


        public ICollection<InvoicePosition> InvoicePositions { get; set; }
        public ApplicationUser User { get; set; }
    }
}