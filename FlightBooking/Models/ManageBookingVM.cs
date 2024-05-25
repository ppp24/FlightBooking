namespace FlightBooking.Models
{
    public class ManageBookingVM
    {
        public int BookingId { get; set; }
        public int PassengerId { get; set; }
        public int OutboundFlightId { get; set; }
        public DateTime OutboundDepartTime { get; set; }
        public DateTime OutboundArriveTime { get; set; }
        public int? ReturnFlightId { get; set; }
        public DateTime? ReturnDepartTime { get; set; }
        public DateTime? ReturnArriveTime { get; set; }
        public string OutboundDepartAirport { get; set; }
        public string OutboundArriveAirport { get; set; }
        public string ReturnDepartAirport { get; set; }
        public string ReturnArriveAirport { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string SpecialRequests { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneContact { get; set; }
        public string ConfirmationNumber { get; set; }

        // Define a property for Passenger details
        public PassengerDetails Passenger { get; set; }

        public string OutboundDepartTimeFormatted => OutboundDepartTime.ToString("yyyy-MM-dd HH:mm:ss");
        public string OutboundArriveTimeFormatted => OutboundArriveTime.ToString("yyyy-MM-dd HH:mm:ss");
        public string ReturnDepartTimeFormatted => ReturnDepartTime?.ToString("yyyy-MM-dd HH:mm:ss");
        public string ReturnArriveTimeFormatted => ReturnArriveTime?.ToString("yyyy-MM-dd HH:mm:ss");
    }

    // Define a class for Passenger details
    public class PassengerDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        // Add more properties if needed
    }
}
