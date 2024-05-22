using System.ComponentModel.DataAnnotations;

namespace FlightBooking.Models
{
    public class PassengerDetailsVM
    {
        public int PassengerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public int? LoyalityPoints { get; set; }

        public string Nationality { get; set; }
        public string PassportNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string IssueCountry { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string PhoneContact { get; set; }
    }
}
