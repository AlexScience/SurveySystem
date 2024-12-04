using SurveySystem.Models.Models;

namespace SurveySystem.API.Services.InterfaceServices;

public interface IUserService
{
    Task<User> CreateUserAsync(string username,string email, string password, string fullName);
    Task<User?> GetUserByIdAsync(string userId);
    Task<IEnumerable<User>> GetAllUsersAsync();
}