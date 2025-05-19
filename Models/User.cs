using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;

public class User
{
    public int Id { get; set; }
    [Required]
    public string NameUser { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Address { get; set; }
    public bool IsAdmin { get; set; }
    public DateTime CreatedAt { get; set; }
    public string SecurityStamp { get; set; }

    public void SetPassword(string password)
    {
        using (var hmac = new HMACSHA512())
        {
            SecurityStamp = Convert.ToBase64String(hmac.Key);
            PasswordHash = Convert.ToBase64String(
                hmac.ComputeHash(Encoding.UTF8.GetBytes(password))
            );
        }
    }

    public bool VerifyPassword(string password)
    {
        using (var hmac = new HMACSHA512(Convert.FromBase64String(SecurityStamp)))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(computedHash) == PasswordHash;
        }
    }
}