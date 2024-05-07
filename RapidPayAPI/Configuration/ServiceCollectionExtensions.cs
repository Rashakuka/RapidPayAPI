using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RapidPayAPI.Context;
using RapidPayAPI.EncryptionLibrary;
using RapidPayAPI.Repositories.Cards;
using RapidPayAPI.Repositories.Payments;
using RapidPayAPI.Repositories.Users;
using RapidPayAPI.Services.CreditCards;
using RapidPayAPI.Services.CreditCards.Validations;
using RapidPayAPI.Services.Payments;
using RapidPayAPI.Services.UFEFee;
using RapidPayAPI.Services.Users;
using RapidPayAPI.Services.Users.Mappings;
using System.Reflection;
using System.Text;

namespace RapidPayAPI.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<AesEncryptor>();
            
            var jwtSettings = configuration.GetSection("JwtSettings");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(config =>
            {
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Description = "Please insert the JWT Bearer."
                });
                config.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                  Array.Empty<string>()
                }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<UFEFeeService>()
                .AddAutoMapper(typeof(UsersMappingProfile))
                .AddValidatorsFromAssemblyContaining<CreditCardRequestValidator>()
                .AddScoped<ICreditCardsService, CreditCardsService>()
                .AddScoped<IPaymentsService, PaymentsService>()
                .AddScoped<IUsersService, UsersService>();
        }

        public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            var rapidPayConnectionString = configuration.GetConnectionString("RapidPayAPIDb");

            services.AddDbContext<RapidPayAPIDbContext>(
                options => options.UseSqlServer(rapidPayConnectionString));

            // Data access services
            return services
                .AddScoped<ICreditCardsRepository, CreditCardsRepository>()
                .AddScoped<IPaymentsRepository, PaymentsRepository>()
                .AddScoped<IUsersRepository, UsersRepository>();
        }

        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            return services;
        }
    }
}
