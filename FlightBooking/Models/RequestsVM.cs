namespace FlightBooking.Models
{
    public class RequestsVM
    {

      //  public string EditPlan { get; set; }
        public string ConfirmationNumber { get; set; }
        public string Action {  get; set; } 
        public string Comment {  get; set; } 
        public string? DocumentBase64 { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Now;


      

    }




}

