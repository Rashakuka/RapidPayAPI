using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RapidPayAPI.EncryptionLibrary;
using RapidPayAPI.Services.Users;
using RapidPayAPI.Services.Users.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RapidPayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IConfiguration _configuration;
        private readonly AesEncryptor _encryptor;

        public AuthenticationController(IUsersService usersService, IConfiguration configuration, AesEncryptor encryptor)
        {
            _usersService = usersService;
            _configuration = configuration;
            _encryptor = encryptor;
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

        private bool ValidatePassword(string password, string storedEncryptedPassword)
        {
            var decryptedPassword = _encryptor.DecryptAES(Convert.FromBase64String(storedEncryptedPassword), _configuration["AppSettings:EncryptionKey"]);
            return password == decryptedPassword;
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
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
