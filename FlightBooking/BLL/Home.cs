using FlightBooking.Areas.Identity.Data;
using FlightBooking.Migrations;
using FlightBooking.Models;
using FlightBooking.Services;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using static FlightBooking.BLL.Enums;

namespace FlightBooking.BLL
{
    public class Home
    {
        // public static bool AddPassengerDetails(PassengerDetailsVM model)
        public static int AddPassengerDetails(PassengerDetailsVM model)
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var passenger = new TblPassengerDetails
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Gender = model.Gender,
                        DOB = model.DOB,
                        //LoyalityPoints = model.LoyalityPoints,
                        Nationality = model.Nationality,
                        PassportNumber = model.PassportNumber,
                        ExpiryDate = model.ExpiryDate,
                        IssueCountry = model.IssueCountry,
                        City = model.City,
                        Country = model.Country,
                        Email = model.Email,
                        PhoneContact = model.PhoneContact
                    };
                    // Check if LoyalityPoints is provided
                    if (model.LoyalityPoints != null)
                    {
                        passenger.LoyalityPoints = model.LoyalityPoints;
                    }

                    db.TblPassengerDetails.Add(passenger);
                    db.SaveChanges();

                    return passenger.PassengerId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static int SaveBookingDetails(BookingDetailsVM model)
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var booking = new TblBooking
                    {
                        PassengerId = model.PassengerId,
                        OutboundFlightId = model.OutboundFlightId,
                        ReturnFlightId = model.ReturnFlightId,
                        OutboundDepartAirport = model.OutboundDepartAirport,
                        OutboundArriveAirport = model.OutboundArriveAirport,
                        OutboundDepartTime = model.OutboundDepartTime,
                        OutboundArriveTime = model.OutboundArriveTime,
                        ReturnDepartAirport = model.ReturnDepartAirport,
                        ReturnArriveAirport = model.ReturnArriveAirport,
                        ReturnDepartTime = model.ReturnDepartTime,
                        ReturnArriveTime = model.ReturnArriveTime,
                        OutboundPriceType = model.OutboundPriceType,
                        OutboundPrice = model.OutboundPrice,
                        ReturnPriceType = model.ReturnPriceType,
                        ReturnPrice = model.ReturnPrice,
                        BookingDate = DateTime.Now,
                        TotalAmount = model.TotalAmount,
                        PaymentStatus = "Successful",
                        SpecialRequests = model.SpecialRequests
                    };


                    db.TblBooking.Add(booking);
                    db.SaveChanges();

                    //return true;
                    return booking.BookingId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public static bool PaymentProcess(PaymentDetailsVM model)
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var payments = new TblPayments
                    {
                        CardNumber = model.CardNumber,
                        NameOnCard = model.NameOnCard,
                        ExpiryMonth = model.ExpiryMonth,
                        ExpiryYear = model.ExpiryYear,
                        CVV = model.CVV,
                        Amount = model.Amount,
                        PaymentDate = DateTime.Now,
                        PaymentStatus = model.PaymentStatus,
                        BookingId = model.BookingId,
                    };
                    db.TblPayments.Add(payments);
                    db.SaveChanges();

                    // If payment is successful, generate a confirmation number
                    if (model.PaymentStatus == "Successful")
                    {
                        var booking = db.TblBooking.FirstOrDefault(b => b.BookingId == model.BookingId);
                        if (booking != null)
                        {
                            booking.ConfirmationNumber = GenerateConfirmationNumber();
                            db.SaveChanges();

                            var passenger = db.TblPassengerDetails.FirstOrDefault(p => p.PassengerId == booking.PassengerId);
                            if (passenger != null)
                            {
                                SendConfirmationEmail(passenger.Email, booking.ConfirmationNumber, model.BookingId);
                            }

                        }
                    }


                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private static string GenerateConfirmationNumber()
        {
            // Generate a unique confirmation number
            // You can customize this method as needed
            return "CONF-" + Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
        }

        private static void SendConfirmationEmail(string toEmail, string confirmationNumber, int bookingId)
        {
            var emailService = new EmailService(); // Assuming you have an EmailService class
            string subject = "Booking Confirmation";
            string body = GenerateEmailBody(confirmationNumber, bookingId);

            emailService.SendEmailAsync(toEmail, subject, body);
        }

        private static string GenerateEmailBody(string confirmationNumber, int bookingId)
        {
            return $@"
        <html>
        <body>
            <h1>Booking Confirmation</h1>
            <p>Dear Customer,</p>
            <p>Thank you for your booking. Your payment has been successfully processed.</p>
            <p><strong>Booking ID:</strong> {bookingId}</p>
            <p><strong>Confirmation Number:</strong> {confirmationNumber}</p>
            <p><strong>You can access your tickets through the system</strong></p>
            <p>We look forward to serving you.</p>
            <p>Best regards,<br/>Pacific Link</p>
        </body>
        </html>";
        }



        //get Itinerary
        public static async Task<ItineraryVM> GetItinerary(int bookingId)
        {
            using (var db = new ApplicationDbContext())
            {
                // Retrieve booking and passenger details in a single query
                var itinerary = await (from b in db.TblBooking
                                       join p in db.TblPassengerDetails on b.PassengerId equals p.PassengerId
                                       where b.BookingId == bookingId
                                       select new ItineraryVM
                                       {
                                           //PassengerId = b.PassengerId,
                                           OutboundFlightId = b.OutboundFlightId,
                                           OutboundDepartTime = b.OutboundDepartTime,
                                           OutboundArriveTime = b.OutboundArriveTime,
                                           ReturnDepartTime = b.ReturnDepartTime,
                                           ReturnArriveTime = b.ReturnArriveTime,
                                           OutboundDepartAirport = b.OutboundDepartAirport,
                                           OutboundArriveAirport = b.OutboundArriveAirport,
                                           ReturnFlightId = (int)b.ReturnFlightId,
                                           ReturnDepartAirport = b.ReturnDepartAirport,
                                           ReturnArriveAirport = b.ReturnArriveAirport,
                                           TotalAmount = b.TotalAmount,
                                           PaymentStatus = b.PaymentStatus,
                                           SpecialRequests = b.SpecialRequests,
                                           FirstName = p.FirstName,
                                           LastName = p.LastName,
                                           Email = p.Email,
                                           PhoneContact = p.PhoneContact,
                                           ConfirmationNumber = b.ConfirmationNumber,
                                       })
                                       .FirstOrDefaultAsync();

                return itinerary;
            }
        }

        //get bookings tbl for customers based on email address
        public static async Task<ManageBookingVM> GetBookingByReference(string confirmationNumber)
        {
            using (var db = new ApplicationDbContext())
            {
                var booking = await db.TblBooking
                    .Where(b => b.ConfirmationNumber == confirmationNumber)
                    .Select(b => new ManageBookingVM
                    {
                        BookingId = b.BookingId,
                        PassengerId = b.PassengerId,
                        OutboundFlightId = b.OutboundFlightId,
                        OutboundDepartTime = b.OutboundDepartTime,
                        OutboundArriveTime = b.OutboundArriveTime,
                        ReturnFlightId = (int?)b.ReturnFlightId,
                        ReturnDepartTime = b.ReturnDepartTime,
                        ReturnArriveTime = b.ReturnArriveTime,
                        OutboundDepartAirport = b.OutboundDepartAirport,
                        OutboundArriveAirport = b.OutboundArriveAirport,
                        ReturnDepartAirport = b.ReturnDepartAirport,
                        ReturnArriveAirport = b.ReturnArriveAirport,
                        TotalAmount = b.TotalAmount,
                        PaymentStatus = b.PaymentStatus,
                        SpecialRequests = b.SpecialRequests,
                        ConfirmationNumber = b.ConfirmationNumber,
                        OutboundPriceType = b.OutboundPriceType,
                        OutboundPrice = b.OutboundPrice,
                        ReturnPriceType = b.ReturnPriceType,
                        ReturnPrice = b.ReturnPrice,

                        // Directly map passenger details to properties in ManageBookingVM
                        FirstName = db.TblPassengerDetails
                            .Where(p => p.PassengerId == b.PassengerId)
                            .Select(p => p.FirstName)
                            .FirstOrDefault(),
                        LastName = db.TblPassengerDetails
                            .Where(p => p.PassengerId == b.PassengerId)
                            .Select(p => p.LastName)
                            .FirstOrDefault(),
                        Email = db.TblPassengerDetails
                            .Where(p => p.PassengerId == b.PassengerId)
                            .Select(p => p.Email)
                            .FirstOrDefault(),
                        PhoneContact = db.TblPassengerDetails
                            .Where(p => p.PassengerId == b.PassengerId)
                            .Select(p => p.PhoneContact)
                            .FirstOrDefault()
                    })
                    .FirstOrDefaultAsync();

                return booking;
            }
        }








    }
}







