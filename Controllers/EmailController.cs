using Cipher;
using InvoiceManager.Models.Domains;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace InvoiceManager.Controllers
{

    public class EmailController : Controller
    {
        private readonly StringCipher _stringCipher = new StringCipher("guidCode");

        public ActionResult Index(string invoiceTitle, string userEmail, string clientEmail)
        {
            var model = PreaperEmailViewModel(invoiceTitle, userEmail, clientEmail);
            return View(model);
        }

        private Email PreaperEmailViewModel(string invoiceTitle, string userEmail, string clientEmail)
        {
            return new Email
            {
                From = userEmail,
                To = clientEmail,
                Subject = $"Faktura-{invoiceTitle}"
            };
        }
        [HttpPost]
        
        public ActionResult SendInvoice(Email emailParameters, HttpPostedFileBase PostedFile)
        {
            var userName = ConfigurationManager.AppSettings["UserName"];
            var password = DecryptSenderEmailPassword();
            var hostSmtp = ConfigurationManager.AppSettings["HostSmtp"];
            var port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
            var enableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
            var senderEmail = emailParameters.From;
            var receiverEmail = emailParameters.To;

            MailMessage message = new MailMessage();
            message.From = new MailAddress(senderEmail, userName);
            message.To.Add(new MailAddress(receiverEmail));
            message.Subject = emailParameters.Subject;
            message.IsBodyHtml = true;
            message.Body = emailParameters.Body;

            if (PostedFile != null)
            {
                string fileName = Path.GetFileName(PostedFile.FileName);
                message.Attachments.Add(new Attachment(PostedFile.InputStream, fileName));
            }

            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;

            SmtpClient mailClient = new SmtpClient();

            try
            {
                mailClient.UseDefaultCredentials = true;
                mailClient.Credentials = new NetworkCredential(senderEmail, password);
                mailClient.Host = hostSmtp;
                mailClient.Port = port;
                mailClient.EnableSsl = enableSsl;
                mailClient.Send(message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                mailClient.Dispose();
            }
            return RedirectToAction("Index");
        }

        private string DecryptSenderEmailPassword()
        {
            var encryptedPassword = ConfigurationManager.AppSettings["UserPassword"];

            if (encryptedPassword.StartsWith("encrypt:"))
            {
                encryptedPassword = _stringCipher.Encrypt(encryptedPassword.Replace("encrypt:", ""));

                var configFile = WebConfigurationManager.OpenWebConfiguration("~");
                configFile.AppSettings.Settings["UserPassword"].Value = encryptedPassword;
                configFile.Save();
            }

            return _stringCipher.Decrypt(encryptedPassword);
        }
    }
}