using FlightBooking.Areas.Identity.Data;
using FlightBooking.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static FlightBooking.BLL.Enums;

namespace FlightBooking.BLL
{
    public class Admin
    {
        private ApplicationDbContext _db;

        public Admin(ApplicationDbContext db)
        {
            _db = db;
        }


        //public static async Task<List<UserRolesVM>> AllUsers(string email, IServiceProvider service)
        //{
        //    try
        //    {
        //        using (ApplicationDbContext db = new ApplicationDbContext())
        //        {
        //            var user = db.ApplicationUser.Where(u => u.Email == email).FirstOrDefault();
        //            var roleManager = service.GetService<RoleManager<ApplicationRoles>>();
        //            var manageRoles = await roleManager.Roles.Where(a => a.Id == user.Id).ToListAsync();
        //            List<UserRolesVM> RolesVMList = new List<UserRolesVM>();
        //            foreach (var role in manageRoles)
        //            {
        //                var roleClaims = await roleManager.GetClaimsAsync(role);
        //                if (roleClaims.Any(c => c.Type == Enums.ClaimTypes.ManageContent))
        //                {
        //                    UserRolesVM rolesVM = new UserRolesVM
        //                    {
        //                        roleid = role.Id,
        //                        roleName = role.Name.Replace("_" + user.company_id, ""),
        //                    };
        //                    RolesVMList.Add(rolesVM);
        //                }
        //            }
        //            var roles = db.Roles.FirstOrDefault(r => r.Name == Roles.Admin.ToString());
        //            UserRolesVM rolesVm = new UserRolesVM
        //            {
        //                roleid = roles.Id,
        //                roleName = Enums.RoleName.admin,
        //            };
        //            RolesVMList.Add(rolesVm);

        //            return RolesVMList;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}





        public static bool AddAirport(AirportVM model)
        {
            try
            {
                var response = false;
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                   
                    var existingAirport = db.TblAirport.FirstOrDefault(a => a.Name == model.Name);
                    if (existingAirport == null)
                    {
                        var newAirport = new TblAirport
                        {
                            Name = model.Name,
                            Location = model.Location,
                        };

                        db.TblAirport.Add(newAirport);
                        db.SaveChanges();
                        response = true;
                    }
                    else
                    {
                        return response;  // Return -1 or some error code to indicate duplication
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it according to your error handling policy
                Console.WriteLine(ex.Message);
                throw ex;  // Return -1 or some error code to indicate failure
            }
        }

        public static bool DeleteAirport(int id)
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    TblAirport tblAirport = db.TblAirport.Find(id);
                    if (tblAirport == null)
                    {
                        
                        return false;
                    }

                    db.TblAirport.Remove(tblAirport);
                    db.SaveChanges(); 
                    return true;
                }
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }

        public static bool AddFlight(FlightVM model)
        {
            try
            {
                var response = false;
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    //var checkflight = db.TblFlight.FirstOrDefault(a => a.FlightNumber == model.FlightNumber);
                    //if (checkflight == null)
                    //{
                        var newFlight = new TblFlight
                        {
                            FlightNumber = model.FlightNumber,
                            FlightName = model.FlightName,
                            EconomyPrice = model.EconomyPrice,
                            BusinessPrice = model.BusinessPrice,
                            DepartureTime = model.DepartureTime,
                            ArrivalTime = model.ArrivalTime,
                            DepartureAirportId = model.DepartureAirportId,
                            ArrivalAirportId = model.ArrivalAirportId,
                            Status = (int)Enums.FStatus.Scheduled,
                        };
                        db.TblFlight.Add(newFlight);
                        db.SaveChanges();
                        response = true;
                    //}
                    //else
                    //{
                    //    return false;  
                    //}
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        public static bool UpdateFlight(FlightVM model)
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var existingFlight = db.TblFlight.FirstOrDefault(a => a.FlightNumber == model.FlightNumber);
                    if (existingFlight != null)
                    {
                        // Update existing flight information
                        existingFlight.FlightName = model.FlightName;
                        existingFlight.EconomyPrice = model.EconomyPrice;
                        existingFlight.BusinessPrice = model.BusinessPrice;
                        existingFlight.DepartureTime = model.DepartureTime;
                        existingFlight.ArrivalTime = model.ArrivalTime;
                        existingFlight.DepartureAirportId = model.DepartureAirportId;
                        existingFlight.ArrivalAirportId = model.ArrivalAirportId;

                        // Optionally, you can update the flight status too if needed
                        existingFlight.Status = (int)Enums.FStatus.Scheduled;

                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        // Flight with the given flight number does not exist, cannot update
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<FlightVM> GetFlightById (int flightId)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var flight = await db.TblFlight
                                .Include(f => f.DepartureAirport)
                                .Include(f => f.ArrivalAirport)
                                .FirstOrDefaultAsync(f => f.FlightId == flightId);
                    if(flight == null)
                    {
                        return null;
                    }
                    var currentTime = DateTime.UtcNow;

                    if (flight.Status == (int)FStatus.Scheduled && currentTime > flight.DepartureTime)
                    {
                        flight.Status = (int)FStatus.Departed;
                    }
                    else if (flight.Status == (int)FStatus.Departed && currentTime > flight.ArrivalTime)
                    {
                        flight.Status = (int)FStatus.Landed;
                    }

                    await db.SaveChangesAsync();
                    var flightVM = new FlightVM
                    {
                        FlightId = flight.FlightId,
                        FlightName = flight.FlightName,
                        FlightNumber = flight.FlightNumber,
                        DepartureAirport = flight.DepartureAirport.Name,
                        ArrivalAirport = flight.ArrivalAirport.Name,
                        DepartureAirportId = flight.DepartureAirportId,
                        ArrivalAirportId = flight.ArrivalAirportId,
                        DepartureTime = flight.DepartureTime,
                        ArrivalTime = flight.ArrivalTime,
                        EconomyPrice = flight.EconomyPrice,
                        BusinessPrice = flight.BusinessPrice,
                        Status = ((FStatus)flight.Status).ToString(),
                    };

                    return flightVM;
                }
            
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }



        public static async Task<List<FlightVM>> GetFlights()
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var currentTime = DateTime.UtcNow;
                    var flightsList = await db.TblFlight.Include(f => f.DepartureAirport).Include(f => f.ArrivalAirport).ToListAsync();

                    foreach (var flight in flightsList)
                    {
                        if (flight.Status == (int)FStatus.Scheduled && currentTime > flight.DepartureTime)
                        {
                            flight.Status = (int)FStatus.Departed;
                        }
                        else if (flight.Status == (int)FStatus.Departed && currentTime > flight.ArrivalTime)
                        {
                           
                            flight.Status = (int)FStatus.Landed;
                        }
                    }


                    await db.SaveChangesAsync();
                   
                    return flightsList.Select(a => new FlightVM
                    {
                        FlightId = a.FlightId,
                        FlightName = a.FlightName,
                        FlightNumber = a.FlightNumber,                  
                        DepartureAirport = a.DepartureAirport.Name,
                        ArrivalAirport = a.ArrivalAirport.Name,
                        DepartureAirportId = a.DepartureAirportId,
                        ArrivalAirportId = a.ArrivalAirportId,
                        DepartureTime = a.DepartureTime,
                        ArrivalTime = a.ArrivalTime,
                        EconomyPrice = a.EconomyPrice,
                        BusinessPrice = a.BusinessPrice,
                        Status = ((FStatus)a.Status).ToString(),
                    }).ToList();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public static async Task<List<FlightVM>> ScheduledFlights(int departureAirportId, int arrivalAirportId)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var currentTime = DateTime.UtcNow;
                    var scheduledFlightsList = await db.TblFlight
                        .Include(f => f.DepartureAirport)
                        .Include(f => f.ArrivalAirport)
                        .Where(f => f.Status == (int)FStatus.Scheduled &&
                            f.DepartureAirportId == departureAirportId &&
                            f.ArrivalAirportId == arrivalAirportId &&
                            f.DepartureTime > currentTime)
                .ToListAsync();

                    return scheduledFlightsList.Select(a => new FlightVM
                    {
                        FlightId = a.FlightId,
                        FlightName = a.FlightName,
                        FlightNumber = a.FlightNumber,
                        DepartureAirport = a.DepartureAirport.Name,
                        ArrivalAirport = a.ArrivalAirport.Name,
                        DepartureAirportId = a.DepartureAirportId,
                        ArrivalAirportId = a.ArrivalAirportId,
                        DepartureTime = a.DepartureTime,
                        ArrivalTime = a.ArrivalTime,
                        EconomyPrice = a.EconomyPrice,
                        BusinessPrice = a.BusinessPrice,
                        Status = ((FStatus)a.Status).ToString(),
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      










    }








}




