using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using NetHire.Models;
using System.Security.Claims;
using NetHire.DTO.Response;
using NetHire.DTO.Request;
using NetHire.Services;


namespace NetHire.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly NetHireDbContext _context;

        public EducationController(NetHireDbContext context)
        {
            _context = context;
        }

        [HttpPost("AddEducation")]
        public async Task<ActionResult<ApiResponse>> AddEducation(AddEducationDTO educationDTO)
        {
            var education = new Education
            {
                EducationId = Guid.NewGuid(),
                UserId = educationDTO.UserId,
                InstituteName = educationDTO.InstituteName,
                CourseName = educationDTO.CourseName,
                StartYear = educationDTO.StartYear,
                EndYear = educationDTO.EndYear,
                Grade = educationDTO.Grade,
                Location = educationDTO.Location
            };

            _context.Educations.Add(education);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse
            {
                ResponseSuccess = true,
                Status = 200,
                Data = education,
                Message = "Education added successfully"
            });
        }

        [HttpGet("GetEducations")]
        public async Task<ActionResult<ApiResponse>> GetEducations()
        {
            var educations = await _context.Educations.ToListAsync();
            return Ok(new ApiResponse
            {
                ResponseSuccess = true,
                Status = 200,
                Data = educations,
                Message = "Educations retrieved successfully"
            });
        }

        [HttpGet("GetEducation/{id}")]
        public async Task<ActionResult<ApiResponse>> GetEducation(Guid id)
        {
            var education = await _context.Educations.FindAsync(id);

            if (education == null)
            {
                return NotFound(new ApiResponse
                {
                    ResponseSuccess = false,
                    Status = 404,
                    Message = "Education not found"
                });
            }

            return Ok(new ApiResponse
            {
                ResponseSuccess = true,
                Status = 200,
                Data = education,
                Message = "Education retrieved successfully"
            });
        }

        [HttpPut("UpdateEducation/{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateEducation(Guid id, AddEducationDTO educationDTO)
        {
            var education = await _context.Educations.FindAsync(id);

            if (education == null)
            {
                return NotFound(new ApiResponse
                {
                    ResponseSuccess = false,
                    Status = 404,
                    Message = "Education not found"
                });
            }

            education.InstituteName = educationDTO.InstituteName;
            education.CourseName = educationDTO.CourseName;
            education.StartYear = educationDTO.StartYear;
            education.EndYear = educationDTO.EndYear;
            education.Grade = educationDTO.Grade;
            education.Location = educationDTO.Location;

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse
            {
                ResponseSuccess = true,
                Status = 200,
                Data = education,
                Message = "Education updated successfully"
            });
        }

        [HttpDelete("DeleteEducation/{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteEducation(Guid id)
        {
            var education = await _context.Educations.FindAsync(id);

            if (education == null)
            {
                return NotFound(new ApiResponse
                {
                    ResponseSuccess = false,
                    Status = 404,
                    Message = "Education not found"
                });
            }

            _context.Educations.Remove(education);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse
            {
                ResponseSuccess = true,
                Status = 200,
                Message = "Education deleted successfully"
            });
        }
    }
}
