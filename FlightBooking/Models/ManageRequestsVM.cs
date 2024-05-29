using FlightBooking.Areas.Identity.Data;

namespace FlightBooking.Models
{
    public class ManageRequestsVM
    {
        public string ConfirmationNumber { get; set; }
        public List<TblRequests> Requests { get; set; }
    }
}
