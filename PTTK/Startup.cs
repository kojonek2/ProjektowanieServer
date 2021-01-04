using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PTTK.Models;
using PTTK.Services;
using PTTK.Utils;
using System;

namespace PTTK
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PTTKContext>(options => options.UseSqlServer(Configuration.GetConnectionString("PTTKContext")));

            services.AddControllers();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMountainGroupService, MountainGroupService>();
            services.AddScoped<IRoutePointService, RoutePointService>();
            services.AddScoped<IBadgeApplicationService, BadgeApplicationService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
