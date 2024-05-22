using FlightBooking.Areas.Identity.Data;

namespace FlightBooking.Models
{
    public class PaymentDetailsVM
    {
        //public List<TblBooking> Bookings { get; set; }
        //public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentStatus { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string NameOnCard { get; set; }
        public int ExpiryYear { get; set; }
        public int ExpiryMonth { get; set; }
        public int BookingId { get; set; }
        //public int PassengerId { get; set; }
        //public int OutboundFlightId { get; set; }
        //public DateTime OutboundDepartTime { get; set; }
        //public DateTime OutboundArriveTime { get; set; }
        //public DateTime? ReturnDepartTime { get; set; }
        //public DateTime? ReturnArriveTime { get; set; }
        //public string OutboundDepartAirport { get; set; }
        //public string OutboundArriveAirport { get; set; }
        //public string? ReturnDepartAirport { get; set; }
        //public string? ReturnArriveAirport { get; set; }
        //public int ReturnFlightId { get; set; }
        //public decimal TotalAmount { get; set; }
        //public string SpecialRequests { get; set; }
    }

}
