using RapidPayAPI.Services.Users.Models;

namespace RapidPayAPI.Services.Users
{
    public interface IUsersService
    {
        Task<UserResult> GetUserAsync(string userName);
    }
}
