//using Dosermana.Domain.Abstract;
//using Dosermana.Domain.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Dosermana.Domain.Concrete
//{
//    public class EFOrderRepository : IOrderRepository
//    {
//        EFDbContext context = new EFDbContext();

//        public IEnumerable<Order> Orders
//        {
//            get { return context.Orders; }
//        }

//        public void SaveOrder(Order order)
//        {
//            if (order.OrderId == 0)
//                context.Orders.Add(order);
//            else
//            {
//                Order dbEntry = context.Orders.Find(order.OrderId);
//                if (dbEntry != null)
//                {
//                    dbEntry.OrderId = order.OrderId;
//                    dbEntry.UserId = order.UserId;
//                    dbEntry.UserEmail = order.UserEmail;
//                    dbEntry.ProductId = order.ProductId;
//                    dbEntry.Quantity = order.Quantity;
//                    dbEntry.Status = order.Status;
//                    dbEntry.Address = order.Address;
//                    dbEntry.OrderDate = order.OrderDate;
//                    dbEntry.Summary = order.Summary;
//                    dbEntry.Note = order.Note;
//                }
//            }
//            context.SaveChanges();
//        }
//    }
//}

using Dosermana.Domain.Abstract;
using Dosermana.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Dosermana.Domain.Concrete
{
    public class EFOrderRepository : IOrderRepository
    {
        private readonly EFDbContext context;

        public EFOrderRepository(EFDbContext dbContext)
        {
            context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IEnumerable<Order> Orders => context.Orders.Include(o => o.OrderItems.Select(oi => oi.Product));

        public void SaveOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (order.OrderId == 0)
            {
                order.OrderDate = DateTime.Now;
                context.Orders.Add(order);
            }
            else
            {
                Order dbEntry = context.Orders.Find(order.OrderId);
                if (dbEntry != null)
                {
                    dbEntry.UserId = order.UserId;
                    dbEntry.UserEmail = order.UserEmail;
                    dbEntry.Status = order.Status;
                    dbEntry.Address = order.Address;
                    dbEntry.Summary = order.Summary;
                    dbEntry.Note = order.Note;
                }
            }

            context.SaveChanges();
        }
    }
}