using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightBooking.Areas.Identity.Data
{
    [Table("tbl_permissions")]
    public class TblPermissions
    {
        public TblPermissions()
        {
            TblRolePermission = new HashSet<TblRolePermissions>();
        }
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("permission_id")]
        public int PermissionId { get; set; }

        [Column("permission")]
        public string? Permission { get; set; }

        [InverseProperty("TblPermissions")]
        public virtual ICollection<TblRolePermissions> TblRolePermission { get; set; }
    }
}
