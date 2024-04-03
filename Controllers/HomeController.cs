using InvoiceManager.Models;
using InvoiceManager.Models.Domains;
using InvoiceManager.Models.Repositories;
using InvoiceManager.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;

namespace InvoiceManager.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private InvoiceRepository _invoiceRepository = new InvoiceRepository();
        private ClientRepository _clientRepository = new ClientRepository();
        private ProductRepository _productRepository = new ProductRepository();
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var invoices = _invoiceRepository.GetInvoices(userId);

            return View(invoices);
        }

        public ActionResult Invoice(int invoiceId = 0)
        {
            var userId = User.Identity.GetUserId();

            var invoice = invoiceId == 0 ?
                GetNewInvoice(userId) :
                _invoiceRepository.GetInvoice(invoiceId, userId);

            var viewModel = PreaperInvoiceViewModel(invoice, userId);

            return View(viewModel);
        }

        private EditInvoiceViewModel PreaperInvoiceViewModel(Invoice invoice, string userId)
        {
            return new EditInvoiceViewModel
            {
                Invoice = invoice,
                Heading = invoice.Id == 0 ? "Dodawanie nowej faktury" : "Faktura",
                Clients = _clientRepository.GetClients(userId),
                PaymentMethods = _invoiceRepository.GetPaymentMethods()

            };
        }

        private Invoice GetNewInvoice(string userId)
        {
            return new Invoice
            {
                UserId = userId,
                CreateDate = DateTime.Now,
                PaymentDate = DateTime.Now.AddDays(7),
            };
        }

        public ActionResult InvoicePosition(int invoiceId, int invoicePositionId = 0)
        {
            var userId = User.Identity.GetUserId();

            var invoicePosition = invoicePositionId == 0 ?
                GetNewInvoicePosition(invoiceId, invoicePositionId) :
                _invoiceRepository.GetInvoicePosition(invoicePositionId, userId);

            var invoicePositionViewModel = PreaperInvoicePositionViewModel(invoicePosition, userId);

            return View(invoicePositionViewModel);
        }

        private EditInvoicePositionViewModel PreaperInvoicePositionViewModel
            (InvoicePosition invoicePosition, string userId)
        {
            return new EditInvoicePositionViewModel
            {
                InvoicePosition = invoicePosition,
                Heading = invoicePosition.Id == 0 ? "Dodawanie nowej pozycji" : " Edycja Pozycji",
                Products = _productRepository.GetProducts(userId)
            };
        }

        private InvoicePosition GetNewInvoicePosition(int invoiceId, int invoicePositionId)
        {
            return new InvoicePosition
            {
                InvoiceId = invoiceId,
                Id = invoicePositionId,
            };

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Invoice(Invoice invoice)
        {
            var userId = User.Identity.GetUserId();
            invoice.UserId = userId;

            if (!ModelState.IsValid)
            {
                var vm = PreaperInvoiceViewModel(invoice, userId);

                return View("Invoice", vm);
            }


            if (invoice.Id == 0)
                _invoiceRepository.Add(invoice);
            else
                _invoiceRepository.Update(invoice);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InvoicePosition(InvoicePosition invoicePosition)
        {
            var userId = User.Identity.GetUserId();

            if (!ModelState.IsValid)
            {
                var vm = PreaperInvoicePositionViewModel(invoicePosition, userId);
                return View("InvoicePosition", vm);
            }
            var product = _productRepository.GetProduct(invoicePosition.ProductId);

            invoicePosition.Value = product.Price * invoicePosition.Quantity;

            if (invoicePosition.Id == 0)
                _invoiceRepository.AddPosition(invoicePosition, userId);
            else
                _invoiceRepository.UpdatePosition(invoicePosition, userId);

            _invoiceRepository.UpdateInvoiceValue(invoicePosition.InvoiceId, userId);

            return RedirectToAction("Invoice", new { invoiceId = invoicePosition.InvoiceId });
        }

        [HttpPost]
        public ActionResult DeleteInvoice(int invoiceId)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                _invoiceRepository.DeleteInvoice(userId, invoiceId);
            }
            catch (Exception exception)
            {
                // logowanie błędu do pliku np. Serilog albo Nlog
                return Json(new { Success = false, Message = exception.Message });
            }

            return Json(new { Success = true });
        }

        [HttpPost]
        public ActionResult DeletePosition(int positionId, int invoiceId)
        {
            var invoiceValue = 0m;

            try
            {
                var userId = User.Identity.GetUserId();
                _invoiceRepository.DeletePosition(positionId, userId);
                invoiceValue = _invoiceRepository.UpdateInvoiceValue(invoiceId, userId);
            }
            catch (Exception exception)
            {

                // logowanie błędu do pliku np. Serilog albo Nlog
                return Json(new { Success = false, Message = exception.Message }); ;
            }

            return Json(new { Success = true, InvoiceValue = invoiceValue });
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}