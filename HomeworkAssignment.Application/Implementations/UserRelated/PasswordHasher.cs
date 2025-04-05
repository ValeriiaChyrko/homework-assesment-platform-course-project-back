using HomeworkAssignment.Application.Abstractions.Contracts;

namespace HomeworkAssignment.Application.Implementations.UserRelated;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));

        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword))
            throw new ArgumentException("Hashed password cannot be null or empty.", nameof(hashedPassword));

        if (string.IsNullOrWhiteSpace(providedPassword))
            throw new ArgumentException("Provided password cannot be null or empty.", nameof(providedPassword));

        return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
    }
}