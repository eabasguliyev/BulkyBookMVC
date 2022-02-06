using System.Collections.Generic;
namespace StripePayment
{
    public interface ICustomStripe
    {
        CheckoutResponse Checkout(int orderHeaderId, IEnumerable<CartDetail> listCart);
        string GetPaymentStatus(string sessionid);
    }
}
