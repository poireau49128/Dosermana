using System;
using System.Collections.Generic;
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

    public class OrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        private readonly EFDbContext _dbContext;

        public OrderProcessor(EmailSettings settings, EFDbContext dbContext)
        {
            emailSettings = settings;

            _dbContext = dbContext;
        }

        public void ProcessOrder(Cart cart, CurrentUser user)
        {
            decimal summary = 0;
            var order = new Order {
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
                    Order_note = line.Note
                };
                order.OrderItems.Add(orderItem);
                _dbContext.OrderItems.Add(orderItem);
            }
            order.Summary = summary;
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();
        }
    }
}
