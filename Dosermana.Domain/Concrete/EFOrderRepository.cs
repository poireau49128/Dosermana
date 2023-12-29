using Dosermana.Domain.Abstract;
using Dosermana.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dosermana.Domain.Concrete
{
    public class EFOrderRepository : IOrderRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Order> Orders
        {
            get { return context.Orders; }
        }

        public void SaveOrder(Order order)
        {
            if (order.OrderId == 0)
                context.Orders.Add(order);
            else
            {
                Order dbEntry = context.Orders.Find(order.OrderId);
                if (dbEntry != null)
                {
                    dbEntry.OrderId = order.OrderId;
                    dbEntry.UserId = order.UserId;
                    dbEntry.UserEmail = order.UserEmail;
                    dbEntry.ProductId = order.ProductId;
                    dbEntry.Quantity = order.Quantity;
                    dbEntry.Status = order.Status;
                    dbEntry.Address = order.Address;
                    dbEntry.OrderDate = order.OrderDate;
                    dbEntry.Summary = order.Summary;
                }
            }
            context.SaveChanges();
        }
    }
}
