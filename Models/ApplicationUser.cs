using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Pratice.Models;

public class ApplicationUser : IdentityUser
{
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string FullName => $"{FirstName} {LastName}".Trim();
}
