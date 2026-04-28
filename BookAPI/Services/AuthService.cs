using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace BookApi.Services;

// Le JWT est composé :
// d'un Header  : type d'algorithme de signature (HS256)
// du Payload -> claims (données de l'user)
// de la signature : header+payload+clé

public class AuthService(IConfiguration config)
{
    // Durée de validité du token
    private readonly int _expiryHours = config.GetValue<int>("Jwt:ExpiryHours", 24);

    public (string Token, DateTime ExpiresAt) GenerateToken(User user)
    {
        var expiresAt = DateTime.UtcNow.AddHours(_expiryHours);


        // Signature du token avec la clé secrète configurée dans appsettings.json
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["Jwt:Key"]
                ?? throw new InvalidOperationException("Jwt:Key manquant dans la config"))
        );
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            //On passe le nom d'utilisateur
            new Claim(ClaimTypes.Name, user.Username),
        };

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
