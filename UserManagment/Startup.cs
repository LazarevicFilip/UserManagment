using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserManagmentDataContext>(options => options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole())).UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));
            services.AddAuthentication(SimulatedAuthenticationOptions.AuthScheme)
               .AddSimulatedAuthentication(
                   userNameidentifier: "foo.bar", // Will be written into ClaimTypes.NameIdentifier
                   userRole: "administrator");    // Will be written into ClaimTypes.Role
            services.AddOpenApiDocument(doc =>
            {
                doc.Title = "User Management API";
                doc.Description = "Demo API for C# course of HTL Leonding";
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseOpenApi();
            app.UseSwaggerUi3();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
