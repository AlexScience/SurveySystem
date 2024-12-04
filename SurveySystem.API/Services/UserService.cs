using Microsoft.AspNetCore.Identity;
using SurveySystem.API.Services.InterfaceServices;
using SurveySystem.Models.Models;

namespace SurveySystem.API.Services;

public class UserService(UserManager<User> userManager) : IUserService
{
    public async Task<User> CreateUserAsync(string username, string email, string password, string fullName)
    {
        var user = new User
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

    public async Task<User?> GetUserByIdAsync(string userId)
    {
        return await userManager.FindByIdAsync(userId);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await Task.FromResult(userManager.Users.ToList());
    }
}