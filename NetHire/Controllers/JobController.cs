using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using NetHire.Models;
using System.Security.Claims;
using NetHire.DTO.Response;
using NetHire.Services;

namespace NetHire.Controllers
{
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
        [HttpGet("GetJobs")]
        public async Task<ActionResult<IEnumerable<Job>>> GetJobs()
        {
            return await _context.Jobs
                .Include(j => j.Company)
                .ToListAsync();
        }

        // GET: api/Job/5
        [HttpGet("GetJob/{id}")]
        public async Task<ActionResult<Job>> GetJob(Guid id)
        {
            var job = await _context.Jobs
                .Include(j => j.Company)
                .FirstOrDefaultAsync(j => j.JobId == id);

            if (job == null)
            {
                return NotFound();
            }

            return job;
        }

        // POST: api/Job
        [HttpPost("CreateJob")]
        public async Task<ActionResult<Job>> CreateJob(Job job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            job.JobId = Guid.NewGuid();
            _context.Jobs.Add(job);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (await JobExists(job.JobId))
                {
                    return Conflict();
                }
                throw;
            }

            return CreatedAtAction(nameof(GetJob), new { id = job.JobId }, job);
        }

        // PUT: api/Job/5
        [HttpPut("UpdateJob/{id}")]
        public async Task<IActionResult> UpdateJob(Guid id, Job job)
        {
            if (id != job.JobId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(job).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await JobExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Job/5
        [HttpDelete("DeleteJob/{id}")]
        public async Task<IActionResult> DeleteJob(Guid id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> JobExists(Guid id)
        {
            return await _context.Jobs.AnyAsync(e => e.JobId == id);
        }
    }
}
