namespace FlightBooking.Areas.Identity.Data
{
    public class TblAuditLog
    {

        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
        public string Details { get; set; }


    }
}
