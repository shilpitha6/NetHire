using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using NetHire.Models;
using System.Security.Claims;
using NetHire.DTO.Response;
using NetHire.Services;

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

        // GET: api/Company
        [HttpGet("GetCompanies")]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            return await _context.Companies.ToListAsync();
        }

        // GET: api/Company/{id}
        [HttpGet("GetCompany/{id}")]
        public async Task<ActionResult<Company>> GetCompany(Guid id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        // POST: api/Company
        [HttpPost("CreateCompany")]
        public async Task<ActionResult<Company>> CreateCompany(Company company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompany), new { id = company.CompanyId }, company);
        }

        // PUT: api/Company/{id}
        [HttpPut("UpdateCompany/{id}")]
        public async Task<IActionResult> UpdateCompany(Guid id, Company company)
        {
            if (id != company.CompanyId)
            {
                return BadRequest();
            }

            _context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Company/{id}
        [HttpDelete("DeleteCompany/{id}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompanyExists(Guid id)
        {
            return _context.Companies.Any(e => e.CompanyId == id);
        }
    }
}
