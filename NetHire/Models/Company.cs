using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetHire.Models
{
    public class Company
    {
        [Key]
        public Guid CompanyId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string CompanyName { get; set; } = null!;

        [StringLength(100)]
        public string? CEO { get; set; }

        public int? FoundedYear { get; set; }

        [StringLength(255)]
        public string? Website { get; set; }

        [StringLength(255)]
        public string? Headquarters { get; set; }

        [Column(TypeName = "decimal(15,2)")]
        public decimal? Revenue { get; set; }

        public int? CompanySize { get; set; }

        [StringLength(100)]
        public string? Industry { get; set; }
    }
}