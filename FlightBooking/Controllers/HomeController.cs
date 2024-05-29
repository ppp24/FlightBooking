using FlightBooking.Areas.Identity.Data;
using FlightBooking.BLL;
using FlightBooking.Models;
using FlightBooking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FlightBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        [Route("/Home/AddPassengerDetails")]
        public JsonResult AddPassengerDetails([FromBody] PassengerDetailsVM model)
        {
            var result = BLL.Home.AddPassengerDetails(model);
            return Json(result);
        }

        //[HttpPost]
        //[Route("/Home/ProcessPayment")]
        //public async Task<JsonResult> ProcessPayment(PaymentDetailsVM model)
        //{
        //    var service = new PaymentService(_context);
        //    var result = await service.ProcessPayment(model);

        //    if (result.Success)
        //    {
        //        return Json(new { success = true, confirmationNumber = result.ConfirmationNumber });
        //    }
        //    else
        //    {
        //        return Json(new { success = false, message = result.Message });
        //    }
        //}

        [HttpPost]
        [Route("/Home/SaveBookingDetails")]
        public JsonResult SaveBookingDetails([FromBody] BookingDetailsVM booking)
        {
            var result = BLL.Home.SaveBookingDetails(booking);
            return Json(result);
        }

        [HttpPost]
        [Route("/Home/PaymentProcess")]
        public JsonResult PaymentProcess([FromBody] PaymentDetailsVM model)
        {
            var result = BLL.Home.PaymentProcess(model);
            return Json(new { success = result });
        }

        [HttpGet]
        [Route("/Home/GetItinerary/{id}")]
        public async Task<IActionResult> GetItinerary(int id)
        {
            var result = await BLL.Home.GetItinerary(id);
            if (result == null)
            {
                return NotFound();
            }
            return Json(result);
        }

        [HttpGet]
        [Route("/Home/GetBookingByReference")]
        public async Task<IActionResult> GetBookingByReference(string confirmationNumber)
        {
            var result = await BLL.Home.GetBookingByReference(confirmationNumber);
            if (result == null)
            {
                return NotFound();
            }
            return Json(result);
        }

        //submit customer request upgrade

        //[HttpPost]
        //[Route("/Home/SubmitRequest")]
        //public JsonResult SubmitRequest([FromBody] PaymentDetailsVM model)
        //{
        //    var result = BLL.Home.SubmitRequest(model);
        //    return Json(new { success = result });
        //}


    }
}