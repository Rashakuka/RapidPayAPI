namespace RapidPayAPI.Repositories.Users
{
    public interface IUsersRepository
    {
        Task<User?> GetUserAsync(string userName);
    }
}
