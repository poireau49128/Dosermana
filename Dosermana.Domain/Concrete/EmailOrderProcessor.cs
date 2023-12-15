using System;
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

        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo, string userID)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials
                    = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod
                        = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                    .AppendLine("Новый заказ обработан")
                    .AppendLine("---")
                    .AppendLine("Товары:");

                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} {2} (итого: {3:c}",
                        line.Quantity, line.Product.Name, line.Product.Color, subtotal);
                }

                body.AppendFormat("Общая стоимость: {0:c}", cart.ComputeTotalValue())
                    .AppendLine("---")
                    .AppendLine("Доставка:")
                    .AppendLine(shippingInfo.Name)
                    .AppendLine(shippingInfo.Line1)
                    .AppendLine(shippingInfo.Line2 ?? "")
                    .AppendLine(shippingInfo.Line3 ?? "")
                    .AppendLine(shippingInfo.City)
                    .AppendLine(shippingInfo.Country)
                    .AppendLine("---");

                MailMessage mailMessage = new MailMessage(
                                       emailSettings.MailFromAddress,	// От кого
                                       emailSettings.MailToAddress,		// Кому
                                       "Новый заказ отправлен!",		// Тема
                                       body.ToString()); 				// Тело письма

                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.UTF8;
                }

                smtpClient.Send(mailMessage);
            }



            foreach (var line in cart.Lines)
            {
                var order = new Order
                {
                    UserId = userID,
                    ProductId = line.Product.ProductId,
                    Quantity = line.Quantity,
                    Status = "В обработке",
                    Address = shippingInfo.Line1,
                    OrderDate = DateTime.Now,
                    Summary = line.Product.Price * line.Quantity
                };

                _dbContext.Orders.Add(order);
            }
            _dbContext.SaveChanges();
        }
    }
}
