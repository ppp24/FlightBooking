namespace FlightBooking.Models
{
    public class ItineraryVM
    {
       // public int PassengerId { get; set; }
        public int OutboundFlightId { get; set; }
        public DateTime OutboundDepartTime { get; set; }
        public DateTime OutboundArriveTime { get; set; }
        public DateTime? ReturnDepartTime { get; set; }
        public DateTime? ReturnArriveTime { get; set; }
        public string OutboundDepartAirport { get; set; }
        public string OutboundArriveAirport { get; set; }
        public string? ReturnDepartAirport { get; set; }
        public string? ReturnArriveAirport { get; set; }
        public int ReturnFlightId { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string SpecialRequests { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneContact { get; set; }
        public string? ConfirmationNumber { get; set; }
    }
}
