using InvoiceManager.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvoiceManager.Models.ViewModels
{
    public class AddEditProductViewModel
    {
        public string Heading { get; set; }
        public Product Product { get; set; }

        public string UserId { get; set; }
    }
}