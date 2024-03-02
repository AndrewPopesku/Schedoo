using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Schedoo.Server.Data;
using Schedoo.Server.Models;

namespace Schedoo.Server.Controllers;

[Route("[controller]")]
[ApiController]
public class AccountController(
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager,
    IConfiguration configuration,
    SchedooContext schedooContext
    ) : ControllerBase
{
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await userManager.FindByNameAsync(model.Username);
        if (user is null)
        {
            user = await userManager.FindByEmailAsync(model.Username);
        }
        
        if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
        {
            var userRoles = await userManager.GetRolesAsync(user);
            
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMonths(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                roles = userRoles,
                group = user.GroupId,
                username = user.UserName,
            });
        }
        return Unauthorized();
    }   
    
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var userExists = await userManager.FindByNameAsync(model.Username);
        if (userExists != null)
            return StatusCode(
                StatusCodes.Status500InternalServerError, 
                new Response
                {
                    Status = "Error", 
                    Message = "User already exists!"
                });

        var user = new User()
        {
            Name = model.Username,
            SurName = model.Username,
            Patronymic = model.Username,
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username
        };
        var result = await userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            return StatusCode(
                StatusCodes.Status500InternalServerError, 
                new Response
                {
                    Status = "Error", 
                    Message = "User creation failed! Please check user details and try again."
                });

        return Ok(new Response { Status = "Success", Message = "User created successfully!" });
    }
    
    [HttpGet]
    [Authorize]
    [Route("userprofile")]
    public async Task<IActionResult> GetUserProfileAsync()
    {
        // Get user claims
        var user = await userManager.FindByNameAsync(User.Identity.Name);
        
        if (user == null)
            return NotFound(new
            {
                // TODO: Localization
                ErrorMessage = "User not found"
            });

        if (await userManager.IsInRoleAsync(user, "Student"))
        {
            var attendancesTotal = schedooContext.Attendances
                .Where(a => a.StudentId == user.Id);
            var attendancesPresent = attendancesTotal
                .Where(a => a.AttendanceStatus == AttendanceStatus.Present);
            
            return Ok(new
            {
                name = user.Name,
                surname = user.SurName,
                patronymic = user.Patronymic,
                email = user.Email,
                username = user.UserName,
                phoneNumber = user.PhoneNumber,
                attendancesTotal = attendancesTotal.Count(),
                attendancesPresent = attendancesPresent.Count()
            });
        }
        return Ok(new
        {
            name = user.Name,
            surname = user.SurName,
            patronymic = user.Patronymic,
            email = user.Email,
            username = user.UserName,
            phoneNumber = user.PhoneNumber
        });
    }
}