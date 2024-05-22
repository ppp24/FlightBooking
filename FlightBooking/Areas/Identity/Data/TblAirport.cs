using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightBooking.Areas.Identity.Data
{
    [Table("tbl_airport")]
    public class TblAirport
    {
        [Key]
        public int AirportId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
    }
}
