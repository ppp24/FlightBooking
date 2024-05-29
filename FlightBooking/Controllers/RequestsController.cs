using FlightBooking.Areas.Identity.Data;
using FlightBooking.BLL;
using FlightBooking.Models;
using FlightBooking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Twilio.Http;
using static FlightBooking.BLL.Enums;

namespace FlightBooking.Controllers
{

    public class RequestController : Controller
    {
        private readonly ILogger<RequestController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly EmailService _emailService;

        public RequestController(ILogger<RequestController> logger, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, EmailService emailService)
        {
            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _emailService = emailService;
        }


        [HttpPost]
        [Route("Requests/SubmitRequest")]
        public IActionResult SubmitRequest([FromBody] RequestsVM model)
        {
            if (ModelState.IsValid)
            {
                bool success = SaveRequest(model);
                if (success)
                {
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }

        private bool SaveRequest(RequestsVM model)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var request = new TblRequests
                    {
                        //EditPlan = model.EditPlan,
                        //NewFlightClass = model.NewFlightClass,
                        //ApplyLoyaltyProgram = model.ApplyLoyaltyProgram,
                        //UploadDocuments = model.UploadDocuments,
                        ConfirmationNumber = model.ConfirmationNumber,
                        RequestDate = DateTime.Now,
                        Action = model.Action,
                        Comment = model.Comment,
                    };

                    if (model.DocumentBase64 != null)
                    {
                        request.DocumentPath = SaveFile(model.DocumentBase64);
                        request.UploadDocuments = model.UploadDocuments;
                    }

                    context.TblRequests.Add(request);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                return false;
            }
        }

        private string SaveFile(string base64String)
        {
            try
            {
                string uploadsDirectory = Path.Combine(Environment.CurrentDirectory, "Uploads");

                // Create the directory if it doesn't exist
                if (!Directory.Exists(uploadsDirectory))
                {
                    Directory.CreateDirectory(uploadsDirectory);
                }

                var filePath = Path.Combine(uploadsDirectory, Guid.NewGuid().ToString() + ".txt");
                var bytes = Convert.FromBase64String(base64String);
                System.IO.File.WriteAllBytes(filePath, bytes);
                return filePath;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return null; // or throw the exception if you want to handle it higher up
            }
        }



        [HttpGet("Requests/GetPendingRequests")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> GetPendingRequests()
        {
            var pendingRequests = await _context.TblRequests.Where(r => r.Status == "Pending").ToListAsync();
            return Ok(pendingRequests);
        }
        [HttpGet("Requests/GetActionedRequests")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> GetActionedRequests()
        {
            var actionedRequests = await _context.TblRequests.Where(r => r.Status == "Pending Payment" || r.Status == "Actioned").ToListAsync();
            return Ok(actionedRequests);
        }


        [HttpGet("Requests/GetRequestById/{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> GetRequestById(int id)
        {
            try
            {
                var request = await _context.TblRequests.FindAsync(id);

                if (request == null)
                {
                    return NotFound(); // Return 404 if the request with the specified ID is not found
                }

                return Ok(request); // Return the request details if found
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error"); // Return 500 if an error occurs
            }
        }


        [HttpGet("Requests/GetAllRequestStatus")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult GetAllRequestStatus()
        {
            var requestStatusValues = Enum.GetValues(typeof(RequestStatus))
                                          .Cast<RequestStatus>()
                                          .Select(v => new { Value = (int)v, Name = v.ToString() })
                                          .ToList();
            return Ok(requestStatusValues);
        }

        [HttpPost("/Requests/SaveRequestAdmin")]
        public JsonResult SaveRequestAdmin([FromBody] RequestAdminVM model)
        {
            var requestsBLL = new Requests(_context, _emailService);
            var result = requestsBLL.SaveRequestAdmin(model);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetRequestsByConfirmationNumber(string confirmationNumber)
        {
            if (string.IsNullOrEmpty(confirmationNumber))
            {
                return BadRequest("Confirmation number is required.");
            }

            var requests = await _context.TblRequests
                .Where(r => r.ConfirmationNumber == confirmationNumber && (r.Status == "Pending" || r.Status == "Pending Payment") )
                .ToListAsync();

            if (requests == null || !requests.Any())
            {
                return NotFound("No requests found for the provided confirmation number.");
            }

            return Ok(requests);
        }

        [HttpPost("/Requests/CompletePaymentAndUpdateRecords")]
        public JsonResult CompletePaymentAndUpdateRecords([FromBody] RequestPaymentVM model)
        {
            var requestsBLL = new Requests(_context, _emailService);
            var result = requestsBLL.CompletePaymentAndUpdateRecords(model);
            return Json(result);
        }

        [HttpGet]
        [Route("/Requests/ViewRequestReport")]
        public async Task<IActionResult> ViewRequestReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date cannot be later than end date.");
            }

            var requests = _context.TblRequests
                .Where(r => r.RequestDate >= startDate && r.RequestDate <= endDate)
                .Select(r => new
                {
                    r.Id,
                    r.ConfirmationNumber,
                    r.RequestDate,
                    r.Action,
                    r.DocumentPath,
                    r.ResponseMessage,
                    r.Status,
                   
                })
                .ToList();

            return Ok(requests);
        }
    }

}
