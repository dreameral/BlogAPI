using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogAPI.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LibraryManagerApi.Controllers;

[Route("Login")]
public class TokenController : ApiController
{
    private IConfiguration _configuration;

    public TokenController(IConfiguration config)
    {
        _configuration = config;
    }

    [HttpPost]
    public IActionResult createToken(Dictionary<string, string> loginRequest)
    {
        if (loginRequest.GetValueOrDefault("username") == "admin" && loginRequest.GetValueOrDefault("password") == "admin")
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes
                (_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, loginRequest.GetValueOrDefault("username")),
                    new Claim(JwtRegisteredClaimNames.Email, loginRequest.GetValueOrDefault("username")),
                    new Claim(JwtRegisteredClaimNames.Jti,
                        Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, "ADMIN"),
                    new Claim(ClaimTypes.Role, "USER")
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);
            return Ok(stringToken);
        }

        if (loginRequest.GetValueOrDefault("username") == "user" && loginRequest.GetValueOrDefault("password") == "user")
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes
                (_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, loginRequest.GetValueOrDefault("username")),
                    new Claim(JwtRegisteredClaimNames.Email, loginRequest.GetValueOrDefault("username")),
                    new Claim(JwtRegisteredClaimNames.Jti,
                        Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, "USER")
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);
            return Ok(stringToken);
        }
        return Unauthorized();
    }
}