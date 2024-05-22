using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightBooking.Areas.Identity.Data
{
    [Table("tbl_passengerdetails")]
    public class TblPassengerDetails
    {
        [Key]
        public int PassengerId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        [Range(0, int.MaxValue)]
        public int? LoyalityPoints { get; set; } = 0;

        [Required]
        [StringLength(50)]
        public string Nationality { get; set; }

        [Required]
        [StringLength(20)]
        public string PassportNumber { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [StringLength(50)]
        public string IssueCountry { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string Country { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneContact { get; set; }
        [Required]
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public byte[]?  attachment { get; set; }
    }
}
