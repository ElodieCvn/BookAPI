using BookApi.Data;
using BookApi.DTOs;
using BookApi.Models;
using BookApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuthService _jwt;

        public AuthController(AppDbContext context, AuthService jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        [HttpPost("login")]
        public IActionResult Login(User login)
        {
            // Vérifie si user existe
            var user = _context.Users
                .FirstOrDefault(u => u.Username == login.Username && u.PasswordHash == login.PasswordHash);

            if (user == null)
                return Unauthorized();

            var token = _jwt.GenerateToken(user);

            return Ok(new { token });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginRequest request)
        {
            var user = new User
            {
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, workFactor: 12),
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("Le user est créé");
        }
    }
}