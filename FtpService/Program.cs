using FtpPoller.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

using System.Diagnostics;

namespace FtpPollerService
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                var host = CreateHostBuilder(args, configuration).Build();
                if (args.Contains("run-migrations"))
                {
                    using (var scope = host.Services.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<FtpPollerContext>();
                        db.Database.Migrate();
                    }
                }
                else
                {
                    Debug.Write("init main");
                    host.Run();
                }
            }
            catch (Exception exception)
            {
                Debug.Write(exception, "Stopped program because of exception");
                throw exception;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration configuration) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddDbContext<FtpPollerContext>(options =>
                    {
                        options.UseNpgsql(configuration.GetConnectionString("FtpDbString"));
                    });
                });
    }
}
