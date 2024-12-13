using Microsoft.AspNetCore.Mvc;
using NetHire.Models;
using Microsoft.EntityFrameworkCore;
using NetHire.DTO.Response;
using NetHire.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;



namespace NetHire.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "user")]
    [Route("api/[controller]")]
    public class ApplicationController : ControllerBase
    {
        private readonly NetHireDbContext _context;

        public ApplicationController(NetHireDbContext context)
        {
            _context = context;
        }

        [HttpPost("{jobId}")]
        public async Task<ActionResult<ApiResponse>> AddApplication(ApplicationDTO application, string jobId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var newApplication = new Application
                {
                    UserId = userId,
                    JobId = Guid.Parse(jobId),
                    FirstName = application.FirstName,
                    MiddleName = application.MiddleName,
                    LastName = application.LastName,
                    Phone = application.Phone,
                    AltPhone = application.AltPhone,
                    Email = application.Email,
                    AltEmail = application.AltEmail,
                    StreetAddress = application.StreetAddress,
                    Address2 = application.Address2,
                    City = application.City,
                    State = application.State,
                    ZipCode = application.ZipCode,
                    Status = "Submitted"
                };

                _context.Applications.Add(newApplication);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    ResponseSuccess = true,
                    Status = StatusCodes.Status200OK,
                    Message = "Application added successfully",
                    Data = newApplication
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    ResponseSuccess = false,
                    Status = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse>> UpdateApplication( Application application)
        {
            try
            {
                var existingApplication = await _context.Applications.FindAsync(application.ApplicationId);
                if (existingApplication == null)
                    return NotFound(new ApiResponse
                    {
                        ResponseSuccess = false,
                        Status = StatusCodes.Status404NotFound,
                        Message = "Application not found"
                    });

                _context.Entry(existingApplication).CurrentValues.SetValues(application);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    ResponseSuccess = true,
                    Status = StatusCodes.Status200OK,
                    Message = "Application updated successfully",
                    Data = existingApplication
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    ResponseSuccess = false,
                    Status = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetApplicationById(string id)
        {
            try
            {
                var application = await _context.Applications
                    .FirstOrDefaultAsync(a => a.ApplicationId == Guid.Parse(id));

                if (application == null)
                    return NotFound(new ApiResponse
                    {
                        ResponseSuccess = false,
                        Status = StatusCodes.Status404NotFound,
                        Message = "Application not found"
                    });

                return Ok(new ApiResponse
                {
                    ResponseSuccess = true,
                    Status = StatusCodes.Status200OK,
                    Data = application
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    ResponseSuccess = false,
                    Status = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }


        [HttpGet("user")]
        public async Task<ActionResult<ApiResponse>> GetApplicationsByUserId()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var applications = await _context.Applications
                    .Where(a => a.UserId == userId)
                    .ToListAsync();

                return Ok(new ApiResponse
                {
                    ResponseSuccess = true,
                    Status = StatusCodes.Status200OK,
                    Data = applications
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    ResponseSuccess = false,
                    Status = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpGet("job/{jobId}")]
        public async Task<ActionResult<ApiResponse>> GetApplicationsByJobId(string jobId)
        {
            try
            {
                var applications = await _context.Applications
                    .Where(a => a.JobId == Guid.Parse(jobId))
                    .ToListAsync();

                return Ok(new ApiResponse
                {
                    ResponseSuccess = true,
                    Status = StatusCodes.Status200OK,
                    Data = applications
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    ResponseSuccess = false,
                    Status = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }
    }
}
