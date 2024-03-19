using InvoiceManager.Models.Domains;
using InvoiceManager.Models.Repositories;
using InvoiceManager.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvoiceManager.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ProductRepository _productRepository = new ProductRepository();

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var products = _productRepository.GetProducts();

            return View(products);
        }

        public ActionResult AddEditProduct(int productId = 0)
        {
            var userId = User.Identity.GetUserId();

            var product = productId == 0 ?
                GetNewProduct(userId) :
                _productRepository.GetProduct(productId, userId);

            var viewModel = PreaperProductViewModel(product, userId);



            return View(viewModel);
        }
        private Product GetNewProduct(string userId)
        {
            return new Product
            {
                UserId = userId
            };
        }

        private AddEditProductViewModel PreaperProductViewModel(Product product, string userId)
        {
            return new AddEditProductViewModel
            {
                Product = product,
                UserId = userId,
                Heading = product.Id == 0 ? "Dodawanie produktu" : "Edycja produktu"
            };
        }


        [HttpPost]
        public ActionResult AddEditProduct(Product product)
        {
            var userId = User.Identity.GetUserId();
            product.UserId = userId;

            if (!ModelState.IsValid)
            {
                var vm = PreaperProductViewModel(product, userId);

                return View("Invoice", vm);
            }

            if (product.Id == 0)
                _productRepository.Add(product);
            else
                ViewBag.Message = "Edycja produktu";
            _productRepository.Update(product);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteProduct(int productId)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                _productRepository.DeleteProduct(productId, userId);
            }
            catch (Exception exception)
            {

                // logowanie błędu do pliku np. Serilog albo Nlog
                return Json(new { Success = false, Message = exception.Message });
            }

            return Json(new { Success = true});
        }
    }
}