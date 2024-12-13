using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using NetHire.Models;
using System.Security.Claims;
using NetHire.DTO.Response;
using NetHire.Services;
using NetHire.DTO.Request;
using NetHire.Models;

namespace NetHire.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Company")]
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly NetHireDbContext _context;

        public CompanyController(NetHireDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet("GetCompanies")]
        public async Task<ActionResult<ApiResponse>> GetCompanies()
        {
            var companies = await _context.Companies.ToListAsync();
            return Ok(new ApiResponse 
            { 
                ResponseSuccess = true,
                Status = 200,
                Data = companies,
                Message = "Companies retrieved successfully"
            });
        }

        // GET: api/Company/{id}
        [AllowAnonymous]
        [HttpGet("GetCompany/{id}")]
        public async Task<ActionResult<ApiResponse>> GetCompany(Guid id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound(new ApiResponse 
                { 
                    ResponseSuccess = false,
                    Status = 404,
                    Message = "Company not found"
                });
            }

            return Ok(new ApiResponse 
            { 
                ResponseSuccess = true,
                Status = 200,
                Data = company,
                Message = "Company retrieved successfully"
            });
        }

         [HttpGet("GetCompaniesByUser")]
        public async Task<ActionResult<ApiResponse>> GetCompaniesByUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new ApiResponse
                {
                    ResponseSuccess = false,
                    Status = 401,
                    Message = "User not authenticated"
                });
            }

            var companies = await _context.Companies
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return Ok(new ApiResponse 
            { 
                ResponseSuccess = true,
                Status = 200,
                Data = companies,
                Message = "Companies retrieved successfully"
            });
        }

        // POST: api/Company
        [HttpPost("CreateCompany")]
        public async Task<ActionResult<ApiResponse>> CreateCompany(AddCompanyDTO company)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new ApiResponse 
                { 
                    ResponseSuccess = false,
                    Status = 401,
                    Message = "User not authenticated"
                });
            }

            var newCompany = new Company
            {
                CompanyId = Guid.NewGuid(),
                UserId = userId,
                CompanyName = company.CompanyName,
                CEO = company.CEO,
                FoundedYear = company.FoundedYear,
                Website = company.Website,
                Headquarters = company.Headquarters,
                Revenue = company.Revenue,
                CompanySize = company.CompanySize,
                Industry = company.Industry
            };

            _context.Companies.Add(newCompany);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompany), 
                new { id = newCompany.CompanyId }, 
                new ApiResponse 
                { 
                    ResponseSuccess = true,
                    Status = 201,
                    Data = newCompany,
                    Message = "Company created successfully"
                });
        }

        // PUT: api/Company/{id}
        [HttpPut("UpdateCompany/{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateCompany(Guid id, Company company)
        {
            if (id != company.CompanyId)
            {
                return BadRequest(new ApiResponse 
                { 
                    ResponseSuccess = false,
                    Status = 400,
                    Message = "Invalid company ID"
                });
            }

            _context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new ApiResponse 
                { 
                    ResponseSuccess = true,
                    Status = 200,
                    Message = "Company updated successfully"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
                {
                    return NotFound(new ApiResponse 
                    { 
                        ResponseSuccess = false,
                        Status = 404,
                        Message = "Company not found"
                    });
                }
                throw;
            }
        }

        // DELETE: api/Company/{id}
        [HttpDelete("DeleteCompany/{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteCompany(Guid id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound(new ApiResponse 
                { 
                    ResponseSuccess = false,
                    Status = 404,
                    Message = "Company not found"
                });
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse 
            { 
                ResponseSuccess = true,
                Status = 200,
                Message = "Company deleted successfully"
            });
        }

        private bool CompanyExists(Guid id)
        {
            return _context.Companies.Any(e => e.CompanyId == id);
        }
    }
}
