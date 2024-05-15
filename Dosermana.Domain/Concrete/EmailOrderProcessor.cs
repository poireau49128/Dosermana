using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using Dosermana.Domain.Abstract;
using Dosermana.Domain.Entities;

namespace Dosermana.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "qwertyuiopov@proton.me";
        public string MailFromAddress = "dosermana@proton.me";
        public bool UseSsl = true;
        public string Username = "dosermana@proton.me";
        public string Password = "B7C13F6DC29DF93DB0AF5EAC69C695173AFA";
        public string ServerName = "smtp.elasticemail.com";
        public int ServerPort = 2525;
        public bool WriteAsFile = false;

        //public string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        //public string relativePath = "dosermana_store_emails";
        //public string absolutePath = Path.Combine(baseDirectory, relativePath);

        public string FileLocation;


        

        public EmailSettings()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = "Store_emails";
            string absolutePath = Path.Combine(baseDirectory, relativePath);
            FileLocation = absolutePath;

            
        }
    }

    public class EmailOrderProcessor : IOrderProcessor
    {

        private EmailSettings emailSettings;

        private readonly EFDbContext _dbContext;

        public EmailOrderProcessor(EmailSettings settings, EFDbContext dbContext)
        {
            emailSettings = settings;

            _dbContext = dbContext;
        }

        public bool ChangeProductQuantity(Product product, int quantity)
        {
            Product dbEntry = _dbContext.Products.Find(product.ProductId);
            if (dbEntry != null)
            {
                if (dbEntry.Quantity_Grodno - quantity >= 0)
                {
                    dbEntry.Quantity_Grodno = dbEntry.Quantity_Grodno - quantity;
                    return true;
                }
                dbEntry.Quantity_Grodno = 0;
            }
            return false;
        }

        public bool ProcessOrder(Cart cart, CurrentUser user)
        {
            bool inStock = true;
            decimal summary = 0;
            var order = new Order
            {
                UserId = user.Id,
                UserEmail = user.Email,
                Status = "Ожидание",
                Address = user.Address,
                OrderDate = DateTime.Now,
                OrderItems = new List<OrderItem>()
            };

            foreach (var line in cart.Lines)
            {
                summary += line.Product.Price * line.Quantity * user.Price_coefficient;
                var orderItem = new OrderItem
                {
                    ProductId = line.Product.ProductId,
                    Quantity = line.Quantity,
                };

                if (!ChangeProductQuantity(line.Product, line.Quantity))
                    inStock = false;

                order.OrderItems.Add(orderItem);
                _dbContext.OrderItems.Add(orderItem);
            }
            order.Summary = summary;
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            return inStock;
            //catch (DbEntityValidationException e)
            //{
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        string test1 = $"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:";
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            string test2 = $" - Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"";
            //        }
            //    }
            //    return false;
            //}
            
        }
    }
}
