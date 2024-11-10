using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetHire.Models
{
    public class EmergencyContact
    {
        [Key]
        public Guid EmergencyContactId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [StringLength(100)]
        public string? FirstName { get; set; }

        [StringLength(100)]
        public string? LastName { get; set; }

        [EmailAddress]
        [StringLength(255)]
        public string? Email { get; set; }

        [StringLength(15)]
        public string? Phone { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        // Navigation properties
        // [ForeignKey("UserId")]
        // public virtual User? User { get; set; }

        public virtual ICollection<Application>? Applications { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}