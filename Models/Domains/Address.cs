using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Web.UI;

namespace InvoiceManager.Models.Domains
{
    public class Address
    {
        public Address()
        {
            Clients = new Collection<Client>();
            Users = new Collection<ApplicationUser>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Nr ulicy")]
        public string StreetNumber { get; set; }

        [Required]
        [Display(Name = "Miejscowość")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Kod Pocztowy")]
        public string PostalCode { get; set; }


        public Collection<Client> Clients { get; set; }
        public Collection<ApplicationUser> Users { get; set; }
    }
}