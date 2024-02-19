
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MockDB;
using Options;
using Requests;
using Services;
using Validators;
using Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Globalization;
using System;


namespace FinalProj
{
    public class Program
    {
        public static void Main(string[] args)
        {
            

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<Options.TokenOptions>(
            builder.Configuration.GetSection(Options.TokenOptions.Section));

            builder.Services.Configure<PasswordHashOptions>(
            builder.Configuration.GetSection(PasswordHashOptions.Section));

            builder.Services.AddCors(config =>
            {
                config.AddPolicy("AllowOrigin", options => options
                                                             .AllowAnyOrigin()
                                                             .AllowAnyMethod());
            });

            // Add services to the container.

            var provider = builder.Services.BuildServiceProvider();
            var tokenOptions = provider.GetRequiredService<IOptions<Options.TokenOptions>>();


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Value.Key!));
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                  .AddJwtBearer(options =>
                  {
                      options.RequireHttpsMetadata = false;
                      options.SaveToken = true;

                      options.TokenValidationParameters = new TokenValidationParameters
                      {
                          IssuerSigningKey = securityKey,
                          ValidateIssuerSigningKey = true,

                          ValidateAudience = true,
                          ValidAudience = tokenOptions.Value.Audience,
                          ValidateIssuer = true,
                          ValidIssuer = tokenOptions.Value.Issuer,
                          ValidateLifetime = true
                      };
                  });

            //builder.Services.AddAuthorization(options => options.AddPolicy("AdminOnly", policy => policy.RequireClaim("role", "Admin"))); // <- não consegui usar dessa forma

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IUserRepo, UsersRepo>();
            builder.Services.AddScoped<IItemRepo, ItemsRepo>();
            builder.Services.AddSingleton<PasswordHashOptions>();
            builder.Services.AddSingleton<IHashingService, HashingService>();
            builder.Services.AddSingleton<IJwtService, JwtService>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IItemService, ItemService>();
            builder.Services.AddScoped<IValidator<BaseUserRequest>, UserValidator>();
            builder.Services.AddScoped<IValidator<BaseItemRequest>, ItemValidator>();
            builder.Services.AddSingleton<IValidator<string>, EmailValidator>();
                        
            

            var app = builder.Build();

            
            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            app.UseCors("AllowOrigin");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<ExceptionMiddleware>();
            app.MapControllers();
            app.Run();
        }
    }
}
