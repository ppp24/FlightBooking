using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightBooking.Areas.Identity.Data
{
    [Table("tbl_destinations")]
    public class TblDestinations
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column ("name")]
        public string Name { get; set; }

        [Column ("country")]
        public string Country { get; set; }

        [Column ("fare")]
        public int Fare { get; set; }

        [Column ("type")]
        public string Type { get; set; }

        [Column ("date_created")]
        public DateTime Date_Created { get; set; }

        [Column("date_modified")]
        public DateTime Date_Modified { get; set; }

    }
}
