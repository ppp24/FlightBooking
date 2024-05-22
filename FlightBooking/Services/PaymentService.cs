//using FlightBooking.Areas.Identity.Data;
//using FlightBooking.Models;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace FlightBooking.Services
//{
//    public class PaymentService
//    {
//        private readonly ApplicationDbContext _context;

//        public PaymentService(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<(bool Success, string Message, string ConfirmationNumber)> ProcessPayment(PaymentDetailsVM model)
//        {
//            using var transaction = await _context.Database.BeginTransactionAsync();

//            try
//            {
//                // Step 1: Save Payment
//                var payment = new TblPayments
//                {
//                    CardNumber = model.CardNumber,
//                    NameOnCard = model.NameOnCard,
//                    ExpiryMonth = model.ExpiryMonth,
//                    ExpiryYear = model.ExpiryYear,
//                    CVV = model.CVV,
//                    Amount = model.Amount,
//                    PaymentDate = DateTime.Now
//                };

//                _context.TblPayments.Add(payment);
//                await _context.SaveChangesAsync();

//                // Step 2: Save Bookings
//                var confirmationNumber = GenerateConfirmationNumber();

//                var booking = new TblBooking
//                {
//                    PassengerId = model.PassengerId,
//                    OutboundFlightId = model.OutboundFlightId,
//                    OutboundDepartAirport = model.OutboundDepartAirport,
//                    OutboundArriveAirport = model.OutboundArriveAirport,
//                    OutboundDepartTime = model.OutboundDepartTime,
//                    OutboundArriveTime = model.OutboundArriveTime,
//                    ReturnFlightId = model.ReturnFlightId,
//                    ReturnDepartAirport = model.ReturnDepartAirport,
//                    ReturnArriveAirport = model.ReturnArriveAirport,
//                    ReturnDepartTime = model.ReturnDepartTime,
//                    ReturnArriveTime = model.ReturnArriveTime,
//                    TotalAmount = model.TotalAmount,
//                    SpecialRequests = model.SpecialRequests,

//                };
                

//                await _context.SaveChangesAsync();
//                await transaction.CommitAsync();

//                return (true, "Payment and bookings successful", confirmationNumber);
//            }
//            catch (Exception ex)
//            {
//                await transaction.RollbackAsync();
//                return (false, "An error occurred: " + ex.Message, null);
//            }
//        }

//        private string GenerateConfirmationNumber()
//        {
//            return Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
//        }
//    }
//}
