using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace InvoiceManager.Models.Domains
{
    public class Email
    {
        [DisplayName("Od:")]
        public string From { get; set; }

        [DisplayName("Do")]
        public string To { get; set; }

        [DisplayName("Temat")]
        public string Subject { get; set; }

        [DisplayName("Wiadomość")]
        public string Body { get; set; } = "Dzień Dobry!\r\n" +
            "Dziękujemy za dokonanie zakupów w naszym sklepie.\r\n" +
            "W załączeniu przesyłamy fakturę na zakupione artykuły.\r\n\r\n" +
            "Zespół Phoenix";

        [ValidateNever]
        public string AttachmentUrl { get; set; }
    }
}