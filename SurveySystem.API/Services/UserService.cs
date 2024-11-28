using Microsoft.AspNetCore.Identity;
using SurveySystem.API.Models;
using SurveySystem.API.Services.InterfaceServices;

namespace SurveySystem.API.Services;

public class UserService(UserManager<ApplicationUser> userManager) : IUserService
{
    public async Task<ApplicationUser> CreateUserAsync(string username, string email, string password, string fullName)
    {
        var user = new ApplicationUser
        {
            UserName = username,
            Email = email,
            FullName = fullName
        };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            throw new Exception(
                $"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return user;
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
    {
        return await userManager.FindByIdAsync(userId);
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
    {
        return await Task.FromResult(userManager.Users.ToList());
    }
}