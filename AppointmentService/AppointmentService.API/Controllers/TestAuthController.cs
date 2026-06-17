using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AppointmentService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous] // Public endpoint for testing only
    public class TestAuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TestAuthController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        // GET: api/testauth/token?role=Admin&username=huongg
        [HttpGet("token")]
        public IActionResult GenerateTestToken([FromQuery] string role = "Admin", [FromQuery] string username = "testuser")
        {
            try
            {
                var secretKey = _configuration["Jwt:Secret"] ?? "SharedSecretKeyForBTLMicroservices2026GroupName5ClinicBookingSystem";
                var keyBytes = Encoding.UTF8.GetBytes(secretKey);

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role),
                    new Claim("GroupName", "Group5"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(2), // Token valid for 2 hours
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(securityToken);

                return Ok(new
                {
                    Token = tokenString,
                    TokenType = "Bearer",
                    ExpiresInSeconds = 7200,
                    Role = role,
                    Username = username
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Loi khi tao token: {ex.Message}");
            }
        }
    }
}
