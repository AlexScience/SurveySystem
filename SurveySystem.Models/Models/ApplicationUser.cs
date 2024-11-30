using Microsoft.AspNetCore.Identity;

namespace SurveySystem.Models.Models;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
}