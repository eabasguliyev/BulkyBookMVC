using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
        }

        public void Update(OrderHeader orderHeader)
        {
            dbSet.Update(orderHeader);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var obj = dbSet.FirstOrDefault(o => o.Id == id);

            if (obj == null) return;

            obj.OrderStatus = orderStatus;

            if(paymentStatus != null)
            {
                obj.PaymentStatus = paymentStatus;
            }
        }

        public void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId)
        {
            var obj = dbSet.FirstOrDefault(o => o.Id == id);

            if (obj == null) return;

            obj.SessionId = sessionId;
            obj.PaymentIntentId = paymentIntentId;
        }
    }
}
