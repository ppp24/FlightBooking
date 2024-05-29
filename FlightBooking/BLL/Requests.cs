using FlightBooking.Areas.Identity.Data;
using FlightBooking.Migrations;
using FlightBooking.Models;
using FlightBooking.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace FlightBooking.BLL
{
    public class Requests
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public Requests(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public bool SaveRequestAdmin(RequestAdminVM model)
        {
            try
            {
                var confirmationNumber = model.ConfirmationNumber;
                var action = model.Action;
                // var id = model.RequestId;
                var comment = model.Comment;

                var booking = _context.TblBooking
                    .FirstOrDefault(b => b.ConfirmationNumber == confirmationNumber);

                if (booking == null)
                {
                    SendFailureEmail(booking, "Booking not found.");
                    return false;
                }


                var request = _context.TblRequests
                    .FirstOrDefault(r => r.ConfirmationNumber == confirmationNumber && r.ResponseMessage == null);

                if (request == null)
                {
                    SendFailureEmail(booking, "Request not found or already processed.");
                    return false;
                }



                string responseMessage = "";
                switch (action)
                {
                    
                    case 1: // Change To Value
                        if (booking.OutboundPriceType.Equals("Lite", StringComparison.OrdinalIgnoreCase) ||booking.ReturnPriceType.Equals("lite", StringComparison.OrdinalIgnoreCase) || booking.ReturnPriceType.Equals(" ", StringComparison.OrdinalIgnoreCase))
                        {
                            decimal totalPriceDifference = CalculateTotalPriceDifference(booking);
                            responseMessage = $"To change to Value, you need to pay an additional amount of {totalPriceDifference:C}.";

                            // Save the request with the price difference fvresponse message
                            request.ResponseMessage = responseMessage;
                            request.Status = "Pending Payment";
                            request.NewPaymentAmount = totalPriceDifference;
                            _context.SaveChanges();

                            // Send email to customer with payment details
                            SendPaymentEmail(booking, responseMessage);
                            return true;
                        }
                        else
                        {
                            responseMessage = "No change needed as existing class type is value. Request has been closed";
                            request.ResponseMessage = responseMessage;
                            request.Status = "Actioned";
                            _context.SaveChanges();
                            SendFailureEmail(booking, responseMessage);
                            return true;
                        }


                    case 2: // Change To Lite
                        if (booking.OutboundPriceType.Equals("Value", StringComparison.OrdinalIgnoreCase) || booking.ReturnPriceType.Equals("Value", StringComparison.OrdinalIgnoreCase))
                        {
                            decimal totalPriceDifference = CalculateRefundPrice(booking);
                            responseMessage = $"To change to Lite, you will be refunded  {totalPriceDifference:C} in 5 business days";

                            // Save the request with the price difference fvresponse message
                            request.ResponseMessage = responseMessage;
                            request.Status = "Actioned";
                            request.NewPaymentAmount = totalPriceDifference;
                            booking.OutboundPriceType = "Lite";
                            booking.ReturnPriceType = "Lite";
                            // Ensure changes to booking are tracked
                            _context.Entry(booking).State = EntityState.Modified;
                            _context.SaveChanges();


                            // Send email to customer with payment details
                            SendRefundEmail(booking, responseMessage);
                            return true;
                        }
                        else
                        {
                            responseMessage = "No change needed as existing class type is lite.Request has been closed";
                            request.ResponseMessage = responseMessage;
                            request.Status = "Actioned";
                            _context.SaveChanges();
                            SendFailureEmail(booking, responseMessage);
                            return true;
                        }


                    case 3:
                        // No Change
                        SendSuccessEmail(booking, "No Change action was selected.Request has been closed");
                        break;

                    case 4:
                        // Apply for Loyalty
                        var passengerBookings = _context.TblBooking
                            .Count(b => b.PassengerId == booking.PassengerId && b.BookingDate < DateTime.Now);

                        if (passengerBookings > 0)
                        {
                            var loyaltyCode = GenerateLoyaltyCode();
                            var passenger = _context.TblPassengerDetails
                                .FirstOrDefault(p => p.PassengerId == booking.PassengerId);
                            if (passenger != null)
                            {
                                SendLoyaltyCodeEmail(passenger.Email, loyaltyCode, confirmationNumber);
                                responseMessage = "Loyality code sent via email";
                                request.ResponseMessage = responseMessage;
                                request.Status = "Actioned";
                                _context.SaveChanges();
                                return true;
                            }
                        }
                        else
                        {
                            SendFailureEmail(booking, "Passenger does not qualify for loyalty program.Request has been closed");
                            responseMessage = "Passenger does not qualify for loyalty program.Request has been closed";
                            request.ResponseMessage = responseMessage;
                            request.Status = "Actioned";
                            _context.SaveChanges();
                            return true;
                        }
                        break;

                    case 5:
                        // Handle action 5
                        SendSuccessEmail(booking, "Document successfully uploaded");
                        responseMessage = "Document successfully uploaded";
                        request.ResponseMessage = responseMessage;
                        request.Status = "Actioned";
                        _context.SaveChanges();
                        break;

                    default:
                        SendFailureEmail(booking, "Invalid action selected.");
                        responseMessage = "Invalid action selected.";
                        break;
                }
          

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing the request.", ex);
            }
        }

        public bool CompletePaymentAndUpdateRecords(RequestPaymentVM model)
        {
            if (model != null)
            {


                var booking = _context.TblBooking
                    .FirstOrDefault(b => b.ConfirmationNumber == model.confirmationNumber);

                if (booking == null)
                {
                    SendFailureEmail(booking, "Booking not found.");
                    return false;
                }

                var request = _context.TblRequests
                    .FirstOrDefault(r => r.ConfirmationNumber == model.confirmationNumber && r.Status == "Pending Payment");

                if (request == null)
                {
                    SendFailureEmail(booking, "Request not found or already processed.");
                    return false;
                }
                else
                {
                    booking.OutboundPriceType = "Value";
                    booking.ReturnPriceType = "Value";
                    request.Status = "Actioned";
                    _context.SaveChanges();
                    SendSuccessEmail(booking, "Change To Value successful.");
                    return true;
                }
              
            }
            else
            {
               
                return false;
            }
        }

     

        private decimal CalculateTotalPriceDifference(TblBooking booking)
        {
            decimal outboundPriceDifference = 0;
            if (booking.OutboundPriceType.Equals("Lite", StringComparison.OrdinalIgnoreCase))
            {
                var outboundFlight = _context.TblFlight.FirstOrDefault(f => f.FlightId == booking.OutboundFlightId);
                if (outboundFlight != null)
                {
                    outboundPriceDifference = outboundFlight.ValuePrice - outboundFlight.EconomyPrice;
                }
            }

            decimal returnPriceDifference = 0;
            if (booking.ReturnPriceType.Equals("Lite", StringComparison.OrdinalIgnoreCase) && booking.ReturnFlightId.HasValue)
            {
                var returnFlight = _context.TblFlight.FirstOrDefault(f => f.FlightId == booking.ReturnFlightId.Value);
                if (returnFlight != null)
                {
                    returnPriceDifference = returnFlight.ValuePrice - returnFlight.EconomyPrice;
                }
            }

            return outboundPriceDifference + returnPriceDifference;
        }
        private decimal CalculateRefundPrice(TblBooking booking)
        {
            decimal refundAmount = 0;

            // Calculate refund for outbound flight if it is of type "Value"
            if (booking.OutboundPriceType.Equals("Value", StringComparison.OrdinalIgnoreCase))
            {
                var outboundFlight = _context.TblFlight.FirstOrDefault(f => f.FlightId == booking.OutboundFlightId);
                if (outboundFlight != null)
                {
                    refundAmount += outboundFlight.ValuePrice - outboundFlight.EconomyPrice;
                }
            }

            // Calculate refund for return flight if it exists and is of type "Value"
            if (booking.ReturnFlightId.HasValue && booking.ReturnPriceType.Equals("Value", StringComparison.OrdinalIgnoreCase))
            {
                var returnFlight = _context.TblFlight.FirstOrDefault(f => f.FlightId == booking.ReturnFlightId.Value);
                if (returnFlight != null)
                {
                    refundAmount += returnFlight.ValuePrice - returnFlight.EconomyPrice;
                }
            }

            return refundAmount;
        }



        private void SendPaymentEmail(TblBooking booking, string message)
        {
            var passenger = _context.TblPassengerDetails
                .FirstOrDefault(p => p.PassengerId == booking.PassengerId);
            if (passenger != null)
            {
                var subject = "Additional Payment Required";
                var confirmationNumber = booking.ConfirmationNumber;
                var body = GeneratePayEmailBody(message, confirmationNumber, booking.TotalAmount + CalculateTotalPriceDifference(booking));
                _emailService.SendEmailAsync(passenger.Email, subject, body);
            }
        }

        private void SendRefundEmail(TblBooking booking, string message)
        {
            var passenger = _context.TblPassengerDetails
                .FirstOrDefault(p => p.PassengerId == booking.PassengerId);
            if (passenger != null)
            {
                var subject = "Request Update";
                var confirmationNumber = booking.ConfirmationNumber;
                var body = GenerateRefundEmailBody(message, confirmationNumber, booking.TotalAmount + CalculateTotalPriceDifference(booking));
                _emailService.SendEmailAsync(passenger.Email, subject, body);
            }
        }

        private static string GenerateRefundEmailBody(string message, string confirmationNumber, decimal newTotalAmount)
        {
            //var ngrokUrl = "https://abcdef.ngrok.io";
            //var paymentLink = $"{ngrokUrl}/paymentrequest.html?confirmationNumber={confirmationNumber}&amount={additionalAmount}";

            var paymentLink = $"http://localhost:31339/paymentrequest.html?confirmationNumber={confirmationNumber}&amount={newTotalAmount}";
            return $@"
        <html>
        <body>
            <h1>Request Update - Changed to Lite</h1>
            <p>For Flight Booking: {confirmationNumber}</p>
            <p>{message}</p>
            <p>Please visit the system for your request update and await refund payment to process in 5 business days</p>
           
            <p>Best regards,<br/>Pacific Link</p>
        </body>
        </html>";
        }
        private static string GeneratePayEmailBody(string message, string confirmationNumber, decimal newTotalAmount)
        {
            //var ngrokUrl = "https://abcdef.ngrok.io";
            //var paymentLink = $"{ngrokUrl}/paymentrequest.html?confirmationNumber={confirmationNumber}&amount={additionalAmount}";

            var paymentLink = $"http://localhost:31339/paymentrequest.html?confirmationNumber={confirmationNumber}&amount={newTotalAmount}";
            return $@"
        <html>
        <body>
            <h1>Booking Request Update</h1>
            <p>For Flight Booking: {confirmationNumber}</p>
            <p>{message}</p>
            <p>Please visit the system to view your request status and proceed for payment:</p>
           
            <p>Best regards,<br/>Pacific Link</p>
        </body>
        </html>";
        }



        private string GenerateLoyaltyCode()
        {
            var random = new Random();
            return random.Next(1000, 9999).ToString();
        }

        private void SendSuccessEmail(TblBooking booking, string message)
        {
            var passenger = _context.TblPassengerDetails
                .FirstOrDefault(p => p.PassengerId == booking.PassengerId);
            if (passenger != null)
            {
                var subject = "Booking Request Success";
                var confirmationNumber = booking.ConfirmationNumber;
                var body = GenerateEmailBody(message, confirmationNumber);
                _emailService.SendEmailAsync(passenger.Email, subject, body);
            }
        }

        private void SendFailureEmail(TblBooking booking, string message)
        {
            var passenger = _context.TblPassengerDetails
                .FirstOrDefault(p => p.PassengerId == booking.PassengerId);
            if (passenger != null)
            {
                var subject = "Booking Request Update";
                var confirmationNumber = booking.ConfirmationNumber;
                var body = GenerateEmailBody(message, confirmationNumber);
                _emailService.SendEmailAsync(passenger.Email, subject, body);
            }
        }

        private void SendLoyaltyCodeEmail(string toEmail, string loyaltyCode, string confirmationNumber)
        {
            var subject = "Your Loyalty Program Code";
            var body = $@"
                <html>
                <body>
                    <h1>Loyalty Program Code</h1>
                    <p>Dear Customer,</p>
                    <p>Booking Confirmation: <strong>{confirmationNumber}</strong> </p>
                    <p>Your loyalty program code is: <strong>{loyaltyCode}</strong></p>
                    <p>Best regards,<br/>Pacific Link</p>
                </body>
                </html>";
            _emailService.SendEmailAsync(toEmail, subject, body);
        }

        private static string GenerateEmailBody(string message, string confirmationNumber)
        {
            return $@"
                <html>
                <body>
                    <h1>Booking Request Update</h1>
                    <p>For Flight Booking: {confirmationNumber}</p>
                    <p>{message}</p>
                    <p>Best regards,<br/>Pacific Link</p>
                </body>
                </html>";
        }
    }
}
