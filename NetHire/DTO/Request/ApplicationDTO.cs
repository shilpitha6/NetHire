using System.ComponentModel.DataAnnotations;

namespace NetHire.DTO.Request
{
    public class ApplicationDTO
    {
        
        public string ApplicationId { get; set; }

        
        public string UserId { get; set; }

        
        public string JobId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = null!;

        [StringLength(100)]
        public string? MiddleName { get; set; }

        [Required] 
        [StringLength(100)]
        public string LastName { get; set; } = null!;

        [StringLength(15)]
        public string? Phone { get; set; }

        [StringLength(15)]
        public string? AltPhone { get; set; }

        [StringLength(255)]
        public string? Email { get; set; }

        [StringLength(255)]
        public string? AltEmail { get; set; }

        [StringLength(255)]
        public string? StreetAddress { get; set; }

        [StringLength(255)]
        public string? Address2 { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? State { get; set; }

        [StringLength(20)]
        public string? ZipCode { get; set; }
    }
}
