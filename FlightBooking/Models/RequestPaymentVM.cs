namespace FlightBooking.Models
{
    public class RequestPaymentVM
    {
        public string confirmationNumber { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string cardName { get; set; }
        public int ExpiryYear { get; set; }
        public int ExpiryMonth { get; set; }
        public decimal newPaymentAmount { get; set; }
    }
}
