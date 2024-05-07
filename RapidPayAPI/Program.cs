using RapidPayAPI.EncryptionLibrary;
using RapidPayAPI.Repositories.UserRoles;
using RapidPayAPI.Repositories.Users;
using RapidPayAPI.Configuration;
using RapidPayAPI.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureApiServices(builder.Configuration)
    .AddBusinessServices()
    .AddDataServices(builder.Configuration)
    .AddPresentationServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var rapidPayDbContext = scope.ServiceProvider.GetRequiredService<RapidPayAPIDbContext>();
    rapidPayDbContext.Database.EnsureCreated();

    if (!rapidPayDbContext.Users.Any())
    {
        // Create roles and users
        var adminRole = new UserRole
        {
            Id = 1,
            Name = "Admin"
        };
        var userRole = new UserRole
        {
            Id = 2,
            Name = "User"
        };

        rapidPayDbContext.UserRoles.AddRange(adminRole, userRole);
        rapidPayDbContext.SaveChanges();

        var encryptionKey = builder.Configuration.GetValue<string>("AppSettings:EncryptionKey");
        var encryptor = new AesEncryptor();

        var adminUser = new User
        {
            Id = 1,
            UserName = "admin",
            Password = Convert.ToBase64String(encryptor.EncryptAES("adminpassword", encryptionKey)),
            UserRoleId = adminRole.Id
        };

        var regularUser = new User
        {
            Id = 2,
            UserName = "user",
            Password = Convert.ToBase64String(encryptor.EncryptAES("userpassword", encryptionKey)),
            UserRoleId = userRole.Id
        };

        rapidPayDbContext.Users.AddRange(adminUser, regularUser);
        rapidPayDbContext.SaveChanges();
    }
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
