using Microsoft.AspNetCore.Identity;

namespace SurveySystem.Models.Models;

public class User : IdentityUser
{
    public string? FullName { get; set; }
}