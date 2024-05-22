//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations;

//namespace FlightBooking.Areas.Identity.Data
//{
//    [Table("tbl_flighthistory")]
//    public class TblFlightHistory
//    {
//        [Key]
//        public int FlightId { get; set; }

//        [Required, StringLength(50)]
//        public string FlightNumber { get; set; }

//        [Required, StringLength(100)]
//        public string FlightName { get; set; }

//        [Column(TypeName = "decimal(18, 2)")]
//        public decimal EconomyPrice { get; set; }

//        [Column(TypeName = "decimal(18, 2)")]
//        public decimal BusinessPrice { get; set; }

//        [Required]
//        public DateTime DepartureTime { get; set; }

//        [Required]
//        public DateTime ArrivalTime { get; set; }

//        [ForeignKey("DepartureAirport")]
//        [ForeignKey("ArrivalAirport")]
//        public int AirportId { get; set; }

//        public virtual TblAirport DepartureAirport { get; set; }
//        public virtual TblAirport ArrivalAirport { get; set; }

//        public int Status { get; set; }
//    }
//}

//}


