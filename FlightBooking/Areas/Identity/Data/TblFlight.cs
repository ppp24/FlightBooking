using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FlightBooking.BLL.Enums;

namespace FlightBooking.Areas.Identity.Data
{
    [Table("tbl_flight")]
    public class TblFlight
    {
        [Key]
        public int FlightId { get; set; }

        [Required, StringLength(50)]
        public string FlightNumber { get; set; }

        [Required, StringLength(100)]
        public string FlightName { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal EconomyPrice { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal BusinessPrice { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [ForeignKey("DepartureAirport")]
        public int DepartureAirportId { get; set; }

        [ForeignKey("ArrivalAirport")]
        public int ArrivalAirportId { get; set; }

        public virtual TblAirport DepartureAirport { get; set; }
        public virtual TblAirport ArrivalAirport { get; set; }

        //public List<TblBooking> Bookings { get; set; }
        // Separate collections for outbound and return bookings
        public virtual ICollection<TblBooking> OutboundBookings { get; set; }
        public virtual ICollection<TblBooking> ReturnBookings { get; set; }


        [NotMapped]
        public TimeSpan Duration => ArrivalTime - DepartureTime;
        public int Status { get; set; }

    }
}
