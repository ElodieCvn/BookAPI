using System.ComponentModel.DataAnnotations;

namespace BookApi.DTOs;

// creation et modif de livre
public record BookRequest(
    [Required, MinLength(1), MaxLength(200)] string Title,
    [Required, MinLength(1), MaxLength(150)] string Author,
    [Range(1000, 2100)] int Year
);

//retour pour l'user
public record BookResponse(
    int Id,
    string Title,
    string Author,
    int Year
);

//DTO pour la connexion et l'inscription.
public record LoginRequest(
    [Required, MinLength(3), MaxLength(50)] string Username,
    [Required, MinLength(6)] string Password
);

// Réponse d'authentification contenant le JWT
public record AuthResponse(
    string Token,
    string Username,
    DateTime ExpiresAt
);
