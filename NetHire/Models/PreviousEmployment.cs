using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetHire.Models
{
    public class PreviousEmployment
    {
        [Key]
        public Guid PreviousEmploymentId { get; set; }

        [Required]
        public string UserId { get; set; }

        [StringLength(255)]
        public string? EmployerName { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }

        [StringLength(100)]
        public string? JobTitle { get; set; }

        public string? JobDescription { get; set; }

        [Column(TypeName = "decimal(15,2)")]
        public decimal? Pay { get; set; }

        [StringLength(255)]
        public string? ReasonForLeaving { get; set; }

        // Navigation properties
       

        public virtual ICollection<Application>? Applications { get; set; }
    }
}