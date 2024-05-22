using FlightBooking.Migrations;
using Microsoft.AspNetCore.Mvc.Rendering;
using static FlightBooking.BLL.Enums;

public class FlightVM
{
    public int FlightId { get; set; }
 
    public string DepartureAirport { get; set; }
    public string ArrivalAirport { get; set; }


    public string FlightName { get; set; }
    public string FlightNumber { get; set; }
    public decimal EconomyPrice { get; set; }
    public decimal BusinessPrice { get; set; }
    public int DepartureAirportId { get; set; }
    public int ArrivalAirportId { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public string Status { get; set; }

}
