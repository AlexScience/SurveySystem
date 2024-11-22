using Microsoft.AspNetCore.Identity;

namespace SurveySystem.API.Models;

public class ApplicationUser: IdentityUser
{
    public string? FullName { get; set; }
}