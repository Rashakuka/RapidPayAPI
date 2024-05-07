using Microsoft.EntityFrameworkCore;
using RapidPayAPI.Repositories.Cards;
using RapidPayAPI.Repositories.Payments;
using RapidPayAPI.Repositories.UserRoles;
using RapidPayAPI.Repositories.Users;

namespace RapidPayAPI.Context
{
    public class RapidPayAPIDbContext : DbContext
    {
        public RapidPayAPIDbContext(DbContextOptions<RapidPayAPIDbContext> options)
            : base(options)
        { }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<CreditCard> CreditCards { get; set; }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CreditCardConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
        }
    }
}
