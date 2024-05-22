using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightBooking.Areas.Identity.Data
{
    [Table("tbl_role_permissions")]
    public class TblRolePermissions
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("role_id")]
        public string? RoleId { get; set; }

        [Column("permission_id")]
        public int permissionId { get; set; }

        public virtual ApplicationRoles? Role { get; set; }
        public virtual TblPermissions? TblPermissions { get; set; }
    }
}
