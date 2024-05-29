using FlightBooking.Areas.Identity.Data;
using FlightBooking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlightBooking.BLL;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using NuGet.Packaging.Signing;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FlightBooking.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context; 


        public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        //Manage Content

        public IActionResult ManageContent()
        {
            ViewBag.activeTab = "mcontent";
            return View();
        }

        public IActionResult ManageRequests()
        {
            ViewBag.activeTab = "mrequests";
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetAirports()
        {
            try
            {
                ApplicationDbContext db = new ApplicationDbContext();
                var airportList = await db.TblAirport.ToListAsync();

                var airports = airportList.Select(a => new AirportVM
                {
                    Id = a.AirportId,
                    Name = a.Name,
                    Location = a.Location
                }).ToList();

                return Ok(airports);
            }
            catch (Exception ex)
            {
                // Log the exception details here to understand the cause
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }



        [HttpPost]
        [Route("AddAirport")]
        public JsonResult AddAirport(AirportVM model)
        {
            var result = BLL.Admin.AddAirport(model);
            return Json(result);

        }

        [HttpPost]
        [Route("DeleteAirport")]
        public JsonResult DeleteAirport(int id)
        {
            bool response;
            response = BLL.Admin.DeleteAirport(id);

            if (response == true)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

       
        [HttpPost]
        [Route("AddFlight")]
        public JsonResult AddFlight([FromBody] FlightVM model)
        {
            var result = BLL.Admin.AddFlight(model);
            return Json(result);
        }

        [HttpPost]
        [Route("UpdateFlight")]
        public JsonResult UpdateFlight([FromBody] FlightVM model)
        {
            var result = BLL.Admin.UpdateFlight(model);
            return Json(result);
        }

        [HttpGet]
        [Route("DropdownAirport")]
        public ActionResult<List<TblAirport>> DropdownAirport()
        {
            using (var db = new ApplicationDbContext())
            {
                var airports = db.TblAirport.Select(a => new SelectListItem
                {
                    Value = a.AirportId.ToString(),  // Assuming AirportId is the identifier
                    Text = a.Name
                }).ToList();

                return Json(airports);
            }
        }

        [HttpGet]
        [Route("DropdownLocation")]
        public ActionResult<List<TblAirport>> DropdownLocation()
        {
            using (var db = new ApplicationDbContext())
            {
                var airports = db.TblAirport.Select(a => new SelectListItem
                {
                    Value = a.AirportId.ToString(),  // Assuming AirportId is the identifier
                    Text = a.Location
                }).ToList();

                return Json(airports);
            }
        }

        [HttpGet]
        [Route("/Admin/GetFlights")]
        public async Task<IActionResult> GetFlights()
        {
            var flights = await BLL.Admin.GetFlights();
            return Json(flights);
        }

        [HttpGet]
        [Route("/Admin/GetFlightId/{id}")]
        public async Task<IActionResult> GetFlightId(int id)
        {
            var flight = await BLL.Admin.GetFlightById(id);
            if (flight == null)
            {
                return NotFound(); 
            }
            return Json(flight);
        }

        [HttpGet]
        [Route("/Admin/ScheduledFlights")]
        public async Task<IActionResult> ScheduledFlights(int departureAirportId, int arrivalAirportId)
        {
            var flights = await BLL.Admin.ScheduledFlights( departureAirportId,  arrivalAirportId);
            return Json(flights);
        }

        //check if user is logged in
        [HttpGet]
        [Route("/Admin/CheckLoginStatus")]
        public IActionResult CheckLoginStatus()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok(new { isLoggedIn = true });
            }
            else
            {
                return Ok(new { isLoggedIn = false });
            }
        }

    }

}