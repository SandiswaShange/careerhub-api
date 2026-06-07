using Microsoft.AspNetCore.Mvc;
using API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.DTOs;
using API.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
//=======================================================Step 1: Verify the identity===================================================
        await Task.Delay(250);

        if (request.Username != "employer" || request.Password != "password123")
        {
            return Unauthorized();//401
        }
//=======================================================Step 2: Build the claims Payload===================================================
        var claims = new[]
        {
            new Claim (JwtRegisteredClaimNames.Sub, request.Username),
            new Claim (ClaimTypes.Role, "Employer")

        };
//=======================================================Step 3: Create the signing credentials===================================================
        var jwtSecretKey = _configuration["Jwt:SecretKey"];
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSecretKey!)
        );  
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//=======================================================Step 4: Construct and sign the token===================================================
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new LoginResponse(tokenString));
    }
//==================================================================================================================================

    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        var username = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var role = User.FindFirstValue(ClaimTypes.Role);

        return Ok(new { username, role });
    }
}