using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using SAS.DTO;
using System;
using System.Threading.Tasks;

namespace SAS
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var logger = NLogBuilder.ConfigureNLog("Nlog.config").GetCurrentClassLogger();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var userManager = services.GetRequiredService<UserManager<UserDTO>>();
                    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var applicationContext = services.GetRequiredService<ApplicationContext>();
                    await SampleData.InitializeAsync(userManager, rolesManager, applicationContext);
                }

                catch (Exception ex)
                {
                    logger.Error(ex, "An error occurred seeding the DB.");
                }

            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
            webBuilder.UseWebRoot("/Home/");
        })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
            })
                .UseNLog();

    }
}