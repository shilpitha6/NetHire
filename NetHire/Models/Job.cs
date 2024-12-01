using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetHire.Models
{
    public enum JobType
    {
        FullTime,
        PartTime,
        Contract,
        Internship
    }

    public enum WorkSetting
    {
        OnSite,
        Remote,
        Hybrid
    }

    public enum TravelRequirement
    {
        None,
        Occasional,
        Frequent
    }

    public enum ApplyType
    {
        Internal,
        External
    }

    public enum JobStatus
    {
        Active,
        Inactive
    }

    public class Job
    {
        [Key]
        public Guid JobId { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; } = null!;

        [StringLength(100)]
        public string? Location { get; set; }

        [Column(TypeName = "decimal(15,2)")]
        public decimal? Salary { get; set; }

        public string? Description { get; set; }

        public JobType? JobType { get; set; }

        public WorkSetting? WorkSettings { get; set; }

        public TravelRequirement? TravelRequirement { get; set; }

        [Required]
        public ApplyType ApplyType { get; set; }

        [Required]
        public JobStatus Status { get; set; } = JobStatus.Active;

        // Navigation property
        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; }
    }
}