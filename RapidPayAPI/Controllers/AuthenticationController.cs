using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RapidPayAPI.EncryptionLibrary;
using RapidPayAPI.Services.Users;
using RapidPayAPI.Services.Users.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace RapidPayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IConfiguration _configuration;
        private readonly HashGeneration _hashGenerator;

        public AuthenticationController(IUsersService usersService, IConfiguration configuration, HashGeneration hashGenerator)
        {
            _usersService = usersService;
            _configuration = configuration;
            _hashGenerator = hashGenerator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserRequest userRequest)
        {
            var userResult = await _usersService.GetUserAsync(userRequest.UserName);

            if (userResult == null || !ValidatePassword(userRequest.Password, userResult.Password))
            {
                return Unauthorized();
            }

            var token = GenerateJwtToken(userResult);

            return Ok(new { Token = token });
        }

        private bool ValidatePassword(string password, string storedHashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHashedPassword);
        }

        private string GenerateJwtToken(UserResult userResult)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userResult.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{userResult.UserName}"),
                new Claim(ClaimTypes.Role, userResult.UserRoleName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration["JwtSettings:AddHourToExpireToken"])), 
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
