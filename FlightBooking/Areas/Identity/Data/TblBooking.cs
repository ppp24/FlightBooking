using FlightBooking.Migrations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightBooking.Areas.Identity.Data
{
    [Table("tbl_booking")]
    public class TblBooking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        [ForeignKey("TblPassengerDetails")]
        public int PassengerId { get; set; }
        public TblPassengerDetails PassengerDetails { get; set; }

        [Required]
        [ForeignKey("OutboundFlight")]
        public int OutboundFlightId { get; set; }
        public TblFlight OutboundFlight { get; set; }

        [ForeignKey("ReturnFlight")]
        public int? ReturnFlightId { get; set; }
        public TblFlight? ReturnFlight { get; set; }

        //public string FlightNumber { get; set; }
        public DateTime OutboundDepartTime { get; set; }
        public DateTime OutboundArriveTime { get; set; }
        public DateTime? ReturnDepartTime { get; set; }
        public DateTime? ReturnArriveTime { get; set; }
        public string OutboundDepartAirport { get; set; }
        public string OutboundArriveAirport { get; set; }
        public string OutboundPriceType { get; set; }
        public int OutboundPrice { get; set; }
        public int ReturnPrice { get; set; }
        public string ReturnPriceType { get; set; }
        public string? ReturnDepartAirport { get; set; }
        public string? ReturnArriveAirport { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(20)]
        public string PaymentStatus { get; set; }

        [StringLength(100)]
        public string SpecialRequests { get; set; }
        [Required]
        public DateTime BookingDate { get; set; } = DateTime.Now;
        public string? ConfirmationNumber { get; set; }
    }

}
