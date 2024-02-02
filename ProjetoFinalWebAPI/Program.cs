
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MockDB;
using Options;
using Requests;
using Services;
using Validators;
using Middlewares;

namespace FinalProj
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MockDB.ItemsRepo.Init(); // inicializa classe "simulando banco de dados"

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<TokenOptions>(
            builder.Configuration.GetSection(TokenOptions.Section));

            builder.Services.Configure<PasswordHashOptions>(
            builder.Configuration.GetSection(PasswordHashOptions.Section));

            builder.Services.AddCors(config =>
            {
                config.AddPolicy("AllowOrigin", options => options
                                                             .AllowAnyOrigin()
                                                             .AllowAnyMethod());
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IUserRepo, UsersRepo>();
            builder.Services.AddSingleton<PasswordHashOptions>();
            builder.Services.AddSingleton<IHashingService, HashingService>();
            builder.Services.AddSingleton<IJwtService, JwtService>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IValidator<BaseUserRequest>, UserValidator>();
            builder.Services.AddSingleton<IValidator<string>, EmailValidator>();

            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization(options => options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Role", "Admin"))); ;

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowOrigin");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();            
            app.MapControllers();
            app.Run();
        }
    }
}
