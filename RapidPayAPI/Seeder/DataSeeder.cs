using RapidPayAPI.Context;
using RapidPayAPI.Repositories.UserRoles;
using RapidPayAPI.Repositories.Users;
using RapidPayAPI.EncryptionLibrary;

public class DataSeeder
{
    private readonly RapidPayAPIDbContext _context;
    private readonly HashGeneration _hashGenerator; 

    public DataSeeder(RapidPayAPIDbContext context, HashGeneration hashGenerator) 
    {
        _context = context;
        _hashGenerator = hashGenerator; 
    }

    public void SeedData()
    {
        _context.Database.EnsureCreated();
        if (!_context.Users.Any())
        {
            var adminRole = new UserRole { Id = 1, Name = "Admin" };
            var userRole = new UserRole { Id = 2, Name = "User" };

            _context.UserRoles.AddRange(adminRole, userRole);
            _context.SaveChanges();

            var adminUser = new User
            {
                Id = 1,
                UserName = "admin",
                Password = _hashGenerator.GenerateHash("adminpassword"), 
                UserRoleId = adminRole.Id
            };

            var regularUser = new User
            {
                Id = 2,
                UserName = "user",
                Password = _hashGenerator.GenerateHash("userpassword"),
                UserRoleId = userRole.Id
            };

            _context.Users.AddRange(adminUser, regularUser);
            _context.SaveChanges();
        }
    }
}
