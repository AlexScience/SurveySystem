using SurveySystem.API.Models;

namespace SurveySystem.API.Services.InterfaceServices;

public interface IUserService
{
    Task<ApplicationUser> CreateUserAsync(string username,string email, string password, string fullName);
    Task<ApplicationUser?> GetUserByIdAsync(string userId);
    Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
}