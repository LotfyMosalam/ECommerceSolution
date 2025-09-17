using ECommerce.Application.Interfaces;
using ECommerce.Core.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ECommerce.Application.DTOs;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ECommerceDbContext _context;
    private readonly IAuthService _tokenService;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly IRefreshTokenRepository _refreshTokenRepo;
    private readonly IConfiguration configuration;

    public AuthController(ECommerceDbContext context, IAuthService tokenService, IRefreshTokenRepository refreshTokenRepo,IConfiguration _configuration)
    {
        _context = context;
        _tokenService = tokenService;
        _refreshTokenRepo = refreshTokenRepo;
        configuration = _configuration;
        _passwordHasher = new PasswordHasher<User>();
    }




    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Username == request.Username || u.Email == request.Email))
            return BadRequest("Username or Email already exists");

        var user = new User
        {
            Username = request.Username,
            Email = request.Email
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "User registered successfully" });
    }




    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (user == null)
            return Unauthorized();

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
            return Unauthorized();

        // 1️⃣ Generate JWT
        var jwt = await _tokenService.GenerateJwtTokenAsync(user);

        // 2️⃣ Generate Refresh Token
        var refreshTokenValue = await _tokenService.GenerateRefreshTokenAsync(user);

        // 3️⃣ Store Refresh Token in DB
        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshTokenValue,
            ExpiresAt = DateTime.UtcNow.AddDays(double.Parse(configuration["Jwt:RefreshTokenExpirationDays"]))
        };
        await _refreshTokenRepo.AddAsync(refreshToken);

        // 4️⃣ Return tokens to client
        return Ok(new { AccessToken = jwt, RefreshToken = refreshTokenValue });
    }


    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        var storedToken = await _refreshTokenRepo.GetByTokenAsync(request.RefreshToken);
        if (storedToken == null || storedToken.ExpiresAt < DateTime.UtcNow)
            return Unauthorized();

        var user = await _context.Users.FindAsync(storedToken.UserId);
        if (user == null) return Unauthorized();

        var newJwt = await _tokenService.GenerateJwtTokenAsync(user);
        // Optionally issue a new refresh token too

        return Ok(new { AccessToken = newJwt });
    }

    public class RefreshRequest
    {
        public string RefreshToken { get; set; } = null!;
    }

}



