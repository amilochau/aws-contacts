using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Milochau.Contacts.Tests.Integration.Apis;

namespace Milochau.Contacts.Tests.Integration
{
    public class Program
    {
        public static string UserId { get; set; } = "545ed070-bed2-4da9-b33a-b66239de4cd9"; // Antoine Milochau - DEV

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.FullName);
            });
            builder.Services.AddCors();

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors(builder =>
            {
                builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:3000");
            });
            app.MapGet("/", () =>
            {
                return Results.Redirect("/swagger");
            }).ExcludeFromDescription();

            app.MapMessagesApis();

            app.Run();
        }
    }
}