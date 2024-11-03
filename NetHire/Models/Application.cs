using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetHire.Models
{
    public class Application
    {
        [Key]
        public Guid ApplicationId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid JobId { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ApplicationDate { get; set; }

        public string? PersonalDetails { get; set; }

        public Guid? EmergencyContactId { get; set; }

        public Guid? EducationId { get; set; }

        public Guid? PreviousEmploymentId { get; set; }

        public Guid? ProfessionalReferenceId { get; set; }

        public byte[]? Documents { get; set; }

        public string? Status { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [ForeignKey("JobId")]
        public virtual Job? Job { get; set; }

        [ForeignKey("EmergencyContactId")]
        public virtual EmergencyContact? EmergencyContact { get; set; }

        [ForeignKey("EducationId")]
        public virtual Education? Education { get; set; }

        [ForeignKey("PreviousEmploymentId")]
        public virtual PreviousEmployment? PreviousEmployment { get; set; }

        [ForeignKey("ProfessionalReferenceId")]
        public virtual ProfessionalReference? ProfessionalReference { get; set; }
    }
}