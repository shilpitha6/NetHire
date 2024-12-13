using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using NetHire.Models;
using System.Security.Claims;
using NetHire.DTO.Response;
using NetHire.Services;
using NetHire.DTO.Request;

namespace NetHire.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Company")]
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly NetHireDbContext _context;

        public JobController(NetHireDbContext context)
        {
            _context = context;
        }

        // GET: api/Job
        [AllowAnonymous]
        [HttpGet("GetJobs")]
        public async Task<ActionResult<ApiResponse>> GetJobs()
        {
            var jobs = await _context.Jobs
                .Include(j => j.Company)
                .ToListAsync();
            
            return Ok(new ApiResponse 
            { 
                ResponseSuccess = true,
                Status = 200,
                Data = jobs,
                Message = "Jobs retrieved successfully"
            });
        }

        [Authorize]
        [HttpGet("GetJobsByUserId")]
        public async Task<ActionResult<ApiResponse>> GetJobsByUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            var jobs = await _context.Jobs
                .Include(j => j.Company)
                .Where(j => j.Company.UserId.ToString() == userId)
                .ToListAsync();

            return Ok(new ApiResponse
            {
                ResponseSuccess = true,
                Status = 200,
                Data = jobs,
                Message = "Jobs retrieved successfully"
            });
        }

        // GET: api/Job/5
        [AllowAnonymous]
        [HttpGet("GetJob/{id}")]
        public async Task<ActionResult<ApiResponse>> GetJob(Guid id)
        {
            var job = await _context.Jobs
                .Include(j => j.Company)
                .FirstOrDefaultAsync(j => j.JobId == id);

            if (job == null)
            {
                return NotFound(new ApiResponse 
                { 
                    ResponseSuccess = false,
                    Status = 404,
                    Message = "Job not found"
                });
            }

            return Ok(new ApiResponse 
            { 
                ResponseSuccess = true,
                Status = 200,
                Data = job,
                Message = "Job retrieved successfully"
            });
        }

        //GET: api/Job/GetJobsByCompanyId
        [AllowAnonymous]
        [HttpGet("GetJobsByCompanyId")]
        public async Task<ActionResult<ApiResponse>> GetJobsByCompanyId(string companyId)
        {
            var jobs = await _context.Jobs
                .Where(j => j.CompanyId.ToString() == companyId)
                .ToListAsync();

            if (jobs == null)
            {
                return NotFound(new ApiResponse 
                { 
                    ResponseSuccess = false,
                    Status = 404,
                    Message = "Jobs not found"
                });
            }

            return Ok(new ApiResponse 
            { 
                ResponseSuccess = true,
                Status = 200,
                Data = jobs,
                Message = "Jobs retrieved successfully"
            });
        }

        // POST: api/Job
        [HttpPost("CreateJob")]
        public async Task<ActionResult<ApiResponse>> CreateJob(AddJobDTO jobDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse 
                { 
                    ResponseSuccess = false,
                    Status = 400,
                    Message = "Invalid model state"
                });
            }

            var job = new Job
            {
                JobId = Guid.NewGuid(),
                CompanyId = jobDTO.CompanyId,
                Title = jobDTO.Title,
                Location = jobDTO.Location,
                Salary = jobDTO.Salary,
                Description = jobDTO.Description,
                JobType = jobDTO.JobType,
                WorkSettings = jobDTO.WorkSettings,
                TravelRequirement = jobDTO.TravelRequirement,
                ApplyType = jobDTO.ApplyType,
                Status = jobDTO.Status  
            };
            _context.Jobs.Add(job);
            
            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetJob), 
                    new { id = job.JobId }, 
                    new ApiResponse 
                    { 
                        ResponseSuccess = true,
                        Status = 201,
                        Data = job,
                        Message = "Job created successfully"
                    });
            }
            catch (DbUpdateException)
            {
                if (await JobExists(job.JobId))
                {
                    return Conflict(new ApiResponse 
                    { 
                        ResponseSuccess = false,
                        Status = 409,
                        Message = "Job already exists"
                    });
                }
                throw;
            }
        }

        // PUT: api/Job/5
        [HttpPut("UpdateJob/{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateJob(Guid id, Job job)
        {
            if (id != job.JobId)
            {
                return BadRequest(new ApiResponse 
                { 
                    ResponseSuccess = false,
                    Status = 400,
                    Message = "Invalid job ID"
                });
            }

            _context.Entry(job).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new ApiResponse 
                { 
                    ResponseSuccess = true,
                    Status = 200,
                    Message = "Job updated successfully"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await JobExists(id))
                {
                    return NotFound(new ApiResponse 
                    { 
                        ResponseSuccess = false,
                        Status = 404,
                        Message = "Job not found"
                    });
                }
                throw;
            }
        }

        // DELETE: api/Job/5
        [HttpDelete("DeleteJob/{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteJob(Guid id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound(new ApiResponse 
                { 
                    ResponseSuccess = false,
                    Status = 404,
                    Message = "Job not found"
                });
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse 
            { 
                ResponseSuccess = true,
                Status = 200,
                Message = "Job deleted successfully"
            });
        }

        private async Task<bool> JobExists(Guid id)
        {
            return await _context.Jobs.AnyAsync(e => e.JobId == id);
        }
    }
}
