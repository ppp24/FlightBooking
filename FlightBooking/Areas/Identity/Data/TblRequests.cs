using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightBooking.Areas.Identity.Data
{
    [Table("tbl_requests")]
    public class TblRequests
    {
        [Key]
        public int Id { get; set; }
        public string ConfirmationNumber { get; set; }
        public string Action { get; set; }
        public string Comment { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; } = "Pending";
       // public string EditPlan { get; set; }
      //  public string NewFlightClass { get; set; }
       // public string ApplyLoyaltyProgram { get; set; }
        public string? UploadDocuments { get; set; }
        public string? DocumentPath { get; set; }
        public string? ResponseMessage { get; set; }
        public decimal? NewPaymentAmount { get; set; }
        //public string AdminComment { get; set; }
       // public DateTime? AdminActionDate { get; set; }
       // public int? AdminId { get; set; }

    }
}
