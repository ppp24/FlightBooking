namespace FlightBooking.BLL
{
    public enum Roles
    {
        Admin,
        Customer,
        Staff,
    }
    public class Enums
    {
        public class RoleName
        {
            public const string admin = "Admin";
            public const string customer = "Customer";
            public const string staff = "Staff";
        }

        public class permissions
        {
            public const int admin = 0;
        }

        public class ClaimTypes
        {
            public const string AccessAdminDash = "Has Access To Admin Dashboard";
            public const string ManageContent = "ManageContent";
            
        }
        public class ClaimValues
        {
            public const string AccessAdminDash = "HasAccessToAdminDashboard";
            public const string ManageStaffs = "ManageEmployees";
            public const string ManageRoles = "ManageRoles";
        }

        public enum FStatus
        {
           Scheduled = 1,
           Departed = 2,
           Landed = 3,
           Cancelled = 4,
           Delayed = 5,
          
        }
        public enum RequestStatus
        {
            ChangeToValue = 1,
            ChangeToLite = 2,
            NoChange = 3,
            ApplyForLoyality = 4,
            UploadDoc = 5,
        }
    }
}
