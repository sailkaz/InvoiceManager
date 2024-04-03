using InvoiceManager.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvoiceManager.Models.Repositories
{
    public class ProductRepository
    {
        public List<Product> GetProducts(string userId)
        {
            using (var context = new ApplicationDbContext()) 
            {
                return context.Products.Where(x => x.UserId == userId).ToList();
            }
        }

        public Product GetProduct(int productId)
        {
            using (var context = new ApplicationDbContext()) 
            {
                return context.Products.Single(x => x.Id == productId);
            }
        }

        public Product GetProduct(int productId, string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                var product = context.Products.Where(x => x.UserId == userId).Single(x => x.Id == productId);
                return product;
            }
        }

        public void Add(Product product)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Products.Add(product);
                context.SaveChanges();
            };
        }

        public void Update(Product product)
        {
            using (var context = new ApplicationDbContext())
            {
                var productToUpdate = context.Products.Single(x => x.Id == product.Id);
                productToUpdate.Id = product.Id;
                productToUpdate.Name = product.Name;
                productToUpdate.Price = product.Price;

                context.SaveChanges();
            };
        }

        internal void DeleteProduct(int productId, string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                var productToDelete = context.Products.Single(x => x.Id == productId && x.UserId == userId);
                context.Products.Remove(productToDelete);
                context.SaveChanges();
            }
        }
    }
}