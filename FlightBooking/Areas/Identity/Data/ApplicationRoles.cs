using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightBooking.Areas.Identity.Data
{
    public class ApplicationRoles : IdentityRole
    {
        public ApplicationRoles() : base()
        {
            TblRolePermissions = new HashSet<TblRolePermissions>();
        }
        public ApplicationRoles(string roleName) : base(roleName)
        {
            TblRolePermissions = new HashSet<TblRolePermissions>();
        }

        [InverseProperty("Role")]
        public virtual ICollection<TblRolePermissions> TblRolePermissions { get; set; }

    }
}
