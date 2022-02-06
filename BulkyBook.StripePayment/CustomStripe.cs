using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace StripePayment
{
    public class CustomStripe:ICustomStripe
    {
        public CustomStripe(string secretKey)
        {
            StripeConfiguration.ApiKey = secretKey;
        }

        public CheckoutResponse Checkout(int orderHeaderId, IEnumerable<CartDetail> listCart)
        {
            var domain = "https://localhost:7213/";

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + "customer/cart/OrderConfirmation?id=" + orderHeaderId,
                CancelUrl = domain + "customer/cart/index",
            };

            foreach(var item in listCart)
            {
                options.LineItems.Add(
                  new SessionLineItemOptions
                  {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                      UnitAmount = (long)(item.Price * 100), // 20.00 -> 2000,
                      Currency = "usd",
                      ProductData = new SessionLineItemPriceDataProductDataOptions
                      {
                        Name = item.Title,
                      },

                    },
                    Quantity = item.Count,
                  });
            }

            var service = new SessionService();
            Session session = service.Create(options);

            return new()
            {
                StatusCode = 303,
                RedirectUrl = session.Url,
                SessionId = session.Id,
                PaymentIntentId = session.PaymentIntentId
            };
        }

        public string GetPaymentStatus(string sessionid)
        {
            var service = new SessionService();
            Session session = service.Get(sessionid);

            return session.PaymentStatus.ToLower();
        }
    }
}
