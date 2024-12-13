using System.ComponentModel.DataAnnotations;

namespace NetHire.DTO.Request
{
    public class AddContactInfoRequestDTO
    {
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
