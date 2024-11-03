using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetHire.Models
{
    public enum UserRole
    {
        Applicant,
        Company,
        Admin
    }

    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string Password { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = null!;

        [Required]
        public UserRole Role { get; set; }

        

        // Navigation properties
        public virtual ApplicantContactInformation? ApplicantContact { get; set; }
        public virtual Company? Company { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}