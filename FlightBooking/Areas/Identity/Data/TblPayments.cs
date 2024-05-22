using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FlightBooking.Areas.Identity.Data
{
    public class TblPayments
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        [ForeignKey("TblBooking")]
        public int BookingId { get; set; }
        //public TblBooking Booking { get; set; }
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string CVV { get; set; }
        [Required]
        public string NameOnCard { get; set; }
        [Required]
        public int ExpiryYear { get; set; }
        [Required]
        public int ExpiryMonth { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        [StringLength(20)]
        public string PaymentStatus { get; set; }
    }
}
