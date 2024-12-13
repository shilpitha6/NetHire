using NetHire.Models;

public class AddJobDTO
{
    public Guid CompanyId { get; set; }
    public string Title { get; set; }
    public string Location { get; set; }
    public decimal Salary { get; set; }
    public string Description { get; set; }
    public string JobType { get; set; }
    public string WorkSettings { get; set; }
    public string TravelRequirement { get; set; }
    public string ApplyType { get; set; }
    public string Status { get; set; }
}
