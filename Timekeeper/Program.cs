using DotNetEnv.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Timekeeper.Components;
using Timekeeper.Services;

namespace Timekeeper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Configuration.AddDotNetEnv();

            var connStr = GetConnectionString();
            builder.Services.AddDbContextPool<TimekeeperContext>(
                o => o.UseSqlServer(connStr)
            );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }

        static string GetConnectionString()
        {
            DotNetEnv.Env.Load();

            var dbServer = DotNetEnv.Env.GetString("DB_SERVER");
            var dbName = DotNetEnv.Env.GetString("DB_NAME");
            var dbUsername = DotNetEnv.Env.GetString("DB_USERNAME");
            var dbPassword = DotNetEnv.Env.GetString("DB_PASSWORD");
            var connStr = $"Server={dbServer};Database={dbName};User Id={dbUsername};Password={dbPassword};TrustServerCertificate=True;";

            return connStr;
        }
    }
}
