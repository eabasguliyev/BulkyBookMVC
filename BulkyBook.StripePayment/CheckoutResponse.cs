namespace StripePayment
{
    public class CheckoutResponse
    {
        public int StatusCode { get; set; }
        public string RedirectUrl { get; set; }
        public string SessionId { get; set; }
        public string PaymentIntentId { get; set; }
    }
}
