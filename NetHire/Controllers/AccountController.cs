using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NetHire.Models;


[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly NetHireDbContext _context;

    public AccountController(NetHireDbContext context)
    {
        _context = context;
    }

    // [HttpPost("register")]
    // public async Task<ActionResult<User>> Register(string email, string password)
    // {
    //     var hmac = new HMACSHA512();

    //     var user = new User
    //     {
    //         FirstName = "firstName",
    //         LastName = "lastName",
    //         Role = UserRole.Applicant,
    //         Password = password,
    //         Email = email,
    //         PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
    //         PasswordSalt = hmac.Key,
    //     };

    //     _context.Users.Add(user);
    //     await _context.SaveChangesAsync();
    //     return user;
    //     }
    //     [HttpPost("login")]
    //     public async Task<ActionResult<User>> Login(string email, string password)
    //     {
    //         if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
    //             return BadRequest("Email and password are required");

    //         var user = await _context.Users
    //             .SingleOrDefaultAsync(x => x.Email == email);

    //         if (user == null)
    //             return Unauthorized("Invalid Email");

    //         var hmac = new HMACSHA512(user.PasswordSalt);
    //         var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

    //         for(int i = 0; i < computedHash.Length; i++)
    //         {
    //             if (computedHash[i] != user.PasswordHash[i])
    //                 return Unauthorized("Invalid Password");
    //         }

    //         return Ok(user);
    //     }
}