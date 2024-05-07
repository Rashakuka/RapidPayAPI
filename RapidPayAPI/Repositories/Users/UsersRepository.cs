using Microsoft.EntityFrameworkCore;
using RapidPayAPI.Context;

namespace RapidPayAPI.Repositories.Users
{
    public class UsersRepository : IUsersRepository
    {
        private readonly RapidPayAPIDbContext _rapidPayDbContext;

        public UsersRepository(RapidPayAPIDbContext rapidPayDbContext)
        {
            _rapidPayDbContext = rapidPayDbContext;
        }

        public async Task<User?> GetUserAsync(string userName)
        {
            return await _rapidPayDbContext.Users
                .Include(user => user.UserRole)
                .FirstOrDefaultAsync(user => user.UserName == userName);
        }
    }
}
