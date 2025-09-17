using ECommerce.Application.Interfaces;
using ECommerce.Core.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.Infrastructure.Services;

public class TokenService : IAuthService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<User?> ValidateUserAsync(string username, string password)
    {
        // Placeholder: actual password validation will be done in AuthController
        throw new NotImplementedException();
    }

    public Task<string> GenerateRefreshTokenAsync(User user)
    {
        // Generate a secure random string (store in DB later)
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        return Task.FromResult(refreshToken);
    }

    public Task<string> GenerateJwtTokenAsync(User user)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:AccessTokenExpirationMinutes"])),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        );

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }
}
