using InvoiceManager.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;

namespace InvoiceManager.Models.Repositories
{
    public class InvoiceRepository
    {
        public List<Invoice> GetInvoices(string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Invoices
                    .Include(x => x.Client)
                    .Where(x => x.UserId == userId)
                    .ToList();
            }
        }

        public Invoice GetInvoice(int invoiceId, string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Invoices

                    .Include(x => x.InvoicePositions)
                    .Include(x => x.InvoicePositions.Select(y => y.Product))
                    .Include(x => x.PaymentMethod)
                    .Include(x => x.User)
                    .Include(x => x.User.Address)
                    .Include(x => x.Client)
                    .Include(x => x.Client.Address)
                    .Single(x => x.UserId == userId && x.Id == invoiceId);

            }
        }

        public List<PaymentMethod> GetPaymentMethods()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.PaymentMethods.ToList();
            }
        }

        public InvoicePosition GetInvoicePosition(int invoicePositionId, string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.InvoicePositions
                    .Include(x => x.Invoice)
                    .Single(x => x.Id == invoicePositionId &&
                        x.Invoice.UserId == userId);
            };
        }

        public void Add(Invoice invoice)
        {
            using (var context = new ApplicationDbContext())
            {
                invoice.CreateDate = DateTime.Now;
                context.Invoices.Add(invoice);
                context.SaveChanges();
            }
        }

        public void Update(Invoice invoice)
        {
            using (var context = new ApplicationDbContext())
            {
                var invoiceToUpdate = context.Invoices
                    .Single(x => x.Id == invoice.Id && x.UserId == invoice.UserId);

                invoiceToUpdate.Title = invoice.Title;
                invoiceToUpdate.ClientId = invoice.ClientId;
                invoiceToUpdate.PaymentMethod = invoice.PaymentMethod;
                invoiceToUpdate.PaymentDate = invoice.PaymentDate;
                invoiceToUpdate.Comments = invoice.Comments;

                context.SaveChanges();
            }
        }

        public void AddPosition(InvoicePosition invoicePosition, string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                var invoice = context.Invoices
                                    .Single(x => x.Id == invoicePosition.InvoiceId &&
                                            x.UserId == userId);
                              
                    context.InvoicePositions.Add(invoicePosition);
                    context.SaveChanges();       
            }
        }

        public void UpdatePosition(InvoicePosition invoicePosition, string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                var positionToUpdate = context.InvoicePositions
                    .Include(x => x.Invoice)
                    .Include(x => x.Product)
                    .Single(x => x.Id == invoicePosition.Id &&
                        x.Invoice.UserId == userId);

                positionToUpdate.Lp = invoicePosition.Lp;
                positionToUpdate.ProductId = invoicePosition.ProductId;
                positionToUpdate.Quantity = invoicePosition.Quantity;
                positionToUpdate.Value = invoicePosition.Product.Price * invoicePosition.Quantity;

                context.SaveChanges();
            }
        }

        public decimal UpdateInvoiceValue(int invoiceId, string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                var invoice = context.Invoices
                    .Include(x => x.InvoicePositions)
                    .Single(x => x.Id == invoiceId && x.UserId == userId);

                invoice.Value = invoice.InvoicePositions.Sum(x => x.Value);
                context.SaveChanges();

                return invoice.Value;
            }
        }


        public void DeleteInvoice(string userId, int invoiceId)
        {
            using (var context = new ApplicationDbContext())
            {
                var invoiceToDelete = context.Invoices
                    .Single(x => x.Id == invoiceId && x.UserId == userId);

                context.Invoices.Remove(invoiceToDelete);
                context.SaveChanges();
            }
        }

        public void DeletePosition(int positionId, string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                var positionToDelete = context.InvoicePositions
                    .Include(x => x.Invoice)
                    .Single(x => x.Id == positionId && x.Invoice.UserId == userId);

                context.InvoicePositions.Remove(positionToDelete);
                context.SaveChanges();
            }
        }
    }
}