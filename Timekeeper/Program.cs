using DotNetEnv.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Web;
using Timekeeper.Components;
using Timekeeper.Services;
using Timekeeper.Services.AuthenticationService;
using Timekeeper.Services.Data;

namespace Timekeeper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            logger.Debug("init main");

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.
                builder.Services.AddRazorComponents()
                    .AddInteractiveServerComponents();
                builder.Services.AddValidation();
                builder.Configuration.AddDotNetEnv();

                // Setup NLog
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                builder.Services.AddScoped<Authentication>();
                builder.Services.AddHttpContextAccessor();
                builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/login";
                    });

                builder.Services.AddAuthorization();
                builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

                builder.Services.AddAntiforgery();

                builder.Services.AddScoped<EmployeeDb>();
                builder.Services.AddScoped<TimekeepingTransactionsDb>();
                var connStr = GetConnectionString();
                builder.Services.AddDbContextPool<TimekeeperContext>(
                    o => o.UseSqlServer(connStr)
                );
                builder.Services.AddDbContextFactory<TimekeeperContext>(
                    options => options.UseSqlServer(connStr));

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
                logger.Info($"Timekeeper initialized at {DateTime.UtcNow.ToString()} (UTC)");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
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
