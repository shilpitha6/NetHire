using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NetHire.Models;
using NetHire.DTO.Request;
using NetHire.DTO.Response;
using NetHire.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NetHire.Services;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly NetHireDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly TokenService _tokenService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    //private readonly UserStore<ApplicationUser> _userStore;

    public AccountController(NetHireDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, TokenService tokenService, SignInManager<ApplicationUser> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _tokenService = tokenService;
        _signInManager = signInManager;
        //_userStore = userStore;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse>> Register(RegisterRequestDTO registerDTO)
    {
        if (registerDTO == null)
        {
            return BadRequest(new ApiResponse
            {
                ResponseSuccess = false,
                Status = 400,
                Message = "Invalid request data"
            });
        }

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDTO.Email);
        if (existingUser != null)
        {
            return BadRequest(new ApiResponse
            {
                ResponseSuccess = false,
                Status = 400,
                Message = "User already exists"
            });
        }

        var user = CreateUser();

        //await _userStore.SetUserNameAsync(user, registerDTO.Email, CancellationToken.None);
        //await _userEmailStore.SetEmailAsync(user, registerDTO.Email, CancellationToken.None);
        user.FirstName = registerDTO.FirstName;
        user.LastName = registerDTO.LastName;
        user.Email = registerDTO.Email;
        user.UserName = registerDTO.Email;

        var result = await _userManager.CreateAsync(user, registerDTO.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, registerDTO.Role);
            var userId = await _userManager.GetUserIdAsync(user);

            var registerResponse = new RegisterResponseDTO
            {
                Id = userId,
                Email = user.Email,
                Role = registerDTO.Role
            };

            return Ok(new ApiResponse
            {
                ResponseSuccess = true,
                Status = 200,
                Message = "User registered successfully",
                Data = registerResponse
            });
        }

        return BadRequest(new ApiResponse
        {
            ResponseSuccess = false,
            Status = 400,
            Message = "Registration failed",
            Data = result.Errors
        });
    }

    private static ApplicationUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<ApplicationUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse>> Login(LoginRequest loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        
        if (user == null) 
            return Unauthorized(new ApiResponse 
            {
                ResponseSuccess = false,
                Status = 401,
                Message = "Invalid email"
            });

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded) 
            return Unauthorized(new ApiResponse 
            {
                ResponseSuccess = false,
                Status = 401,
                Message = "Invalid password"
            });

        var roles = await _userManager.GetRolesAsync(user);
        var userRole = roles.FirstOrDefault();
        
        var token = await _tokenService.CreateToken(user);
        var apiTokenResponse = new ApiTokenResponse
        {
            AccessToken = token,
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = userRole
        };
        return Ok(new ApiResponse 
        {
            ResponseSuccess = true,
            Status = 200,
            Message = "Login successful",
            Data = apiTokenResponse
        });
    }
}