using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Stockimulate
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            Helpers.Constants.ConnectionString = Configuration.GetConnectionString("MS_TableConnectionString");
            Helpers.Constants.PusherAppId = Configuration.GetConnectionString("Pusher_AppId");
            Helpers.Constants.PusherAppSecret = Configuration.GetConnectionString("Pusher_AppSecret");
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

			// Adds a default in-memory implementation of IDistributedCache.
			services.AddDistributedMemoryCache();

			services.AddSession(options =>
			{
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromDays(1);
				options.Cookie.HttpOnly = true;
			});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
                app.UseExceptionHandler("/Main/Error");

            app.UseStaticFiles();

            app.UseSession();

            app.UseMvc(routes => routes.MapRoute(
                "default",
                "{controller=NavPage}/{action=Home}/{id?}"));

        }
    }
}