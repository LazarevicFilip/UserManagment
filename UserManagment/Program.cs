using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagment.Data;

namespace UserManagment
{
    public class Program
    {
        public  static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // If app is called with `fill` as first command line argument,
            // it will fill the connected database with demo data using
            // the `DemoDataGenerator` class.
            if (args.Length > 0 && args[0] == "fill")
            {
                
                using var scope = host.Services.CreateScope();
                using var dc = scope.ServiceProvider.GetRequiredService<UserManagmentDataContext>();


                // Trigger filling of database
                var generator = new DemoDataGenerator(dc);
                await generator.ClearAll();
                await generator.Fill();

                Console.WriteLine("Database has been successfully filled");
                return;
            }
            // We are not in "fill DB with demo data" mode.
            // Therefore, we start the web server for the web API.
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
