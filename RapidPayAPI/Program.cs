using RapidPayAPI.Configuration;

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

if (args.Contains("seed"))
{
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    seeder.SeedData();
    return; 
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
