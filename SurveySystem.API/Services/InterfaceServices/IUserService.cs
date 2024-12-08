using SurveySystem.Models.Models;

namespace SurveySystem.API.Services.InterfaceServices;

public interface IUserService
{
    public Task<User> CreateUserAsync(string username, string email, string password, string fullName);
    public Task<User?> GetUserByIdAsync(string userId);
    public Task<IEnumerable<User>> GetAllUsersAsync();
}