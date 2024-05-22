using FlightBooking.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using static FlightBooking.BLL.Enums;

namespace FlightBooking.Services
{
    public class UpdateStatusService
    {
        public static async Task UpdateFlightStatusAndMoveToHistory()
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var currentTime = DateTime.UtcNow;

                    // Get flights that are still active
                    var activeFlights = await db.TblFlight
                                                 .Where(f => f.DepartureTime <= currentTime && f.Status != (int)FStatus.Landed)
                                                 .ToListAsync();

                    foreach (var flight in activeFlights)
                    {
                        if (currentTime > flight.DepartureTime && flight.Status != (int)FStatus.Departed)
                        {
                            // Update status to Departed
                            flight.Status = (int)FStatus.Departed;
                            db.Entry(flight).State = EntityState.Modified;
                        }
                        else if (currentTime > flight.ArrivalTime && flight.Status != (int)FStatus.Landed)
                        {
                            // Update status to Arrived
                            flight.Status = (int)FStatus.Landed;
                            db.Entry(flight).State = EntityState.Modified;

                            // Move flight to history table
                            //var historyFlight = new HistoryFlight
                            //{
                            //    FlightId = flight.FlightId,
                            //    // Copy other relevant properties from flight to historyFlight
                            //};
                            //db.HistoryFlights.Add(historyFlight);

                            // Remove flight from flights table
                            db.TblFlight.Remove(flight);
                        }
                    }

                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine($"Error updating flight status: {ex.Message}");
            }
        }

    }
}
