using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using NetHire.Models;
using System.Security.Claims;
using NetHire.DTO.Response;
using NetHire.Services;

namespace NetHire.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Applicant")]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactInfoController : ControllerBase
    {
        private readonly NetHireDbContext _context;

        public ContactInfoController(NetHireDbContext context)
        {
            _context = context;
        }

        // GET: api/ContactInfo/GetContactInfoByUserId
        [HttpGet("GetContactInfoByUserId")]
        public async Task<ActionResult<ApiResponse>> GetContactInfoByUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new ApiResponse 
                { 
                    ResponseSuccess = false,
                    Status = 401,
                    Message = "Unauthorized access"
                });
            }

            var contactInfo = await _context.ApplicantContactInformation
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (contactInfo == null)
            {
                return NotFound(new ApiResponse 
                { 
                    ResponseSuccess = false,
                    Status = 404,
                    Message = "Contact information not found"
                });
            }

            return Ok(new ApiResponse 
            { 
                ResponseSuccess = true,
                Status = 200,
                Data = contactInfo,
                Message = "Contact information retrieved successfully"
            });
        }

        // POST: api/ContactInfo/AddContactInfo
        [HttpPost("AddContactInfo")]
        public async Task<ActionResult<ApiResponse>> AddContactInfo(ApplicantContactInformation contactInfo)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            // Check if contact info already exists for this user
            var existingContactInfo = await _context.ApplicantContactInformation
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (existingContactInfo != null)
            {
                return Conflict(new ApiResponse 
                { 
                    ResponseSuccess = false,
                    Status = 409,
                    Message = "Contact information already exists for this user"
                });
            }

            contactInfo.UserId = userId;
            contactInfo.ContactInfoId = Guid.NewGuid();

            _context.ApplicantContactInformation.Add(contactInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetContactInfoByUserId), 
                new { id = contactInfo.ContactInfoId }, 
                new ApiResponse 
                { 
                    ResponseSuccess = true,
                    Status = 201,
                    Data = contactInfo,
                    Message = "Contact information created successfully"
                });
        }

        // PUT: api/ContactInfo/UpdateContactInfo
        [HttpPut("UpdateContactInfo")]
        public async Task<IActionResult> UpdateContactInfo(ApplicantContactInformation contactInfo)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var existingContactInfo = await _context.ApplicantContactInformation
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (existingContactInfo == null)
            {
                return NotFound();
            }

            // Update only the fields that can be modified
            existingContactInfo.Phone = contactInfo.Phone;
            existingContactInfo.AltPhone = contactInfo.AltPhone;
            existingContactInfo.Email = contactInfo.Email;
            existingContactInfo.AltEmail = contactInfo.AltEmail;
            existingContactInfo.StreetAddress = contactInfo.StreetAddress;
            existingContactInfo.Address2 = contactInfo.Address2;
            existingContactInfo.City = contactInfo.City;
            existingContactInfo.State = contactInfo.State;
            existingContactInfo.ZipCode = contactInfo.ZipCode;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactInfoExists(contactInfo.ContactInfoId))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/ContactInfo/DeleteContactInfo
        [HttpDelete("DeleteContactInfo")]
        public async Task<IActionResult> DeleteContactInfo()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var contactInfo = await _context.ApplicantContactInformation
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (contactInfo == null)
            {
                return NotFound();
            }

            _context.ApplicantContactInformation.Remove(contactInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContactInfoExists(Guid id)
        {
            return _context.ApplicantContactInformation.Any(e => e.ContactInfoId == id);
        }
    }
}
