using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stockimulate.Core;
using Stockimulate.Core.Repositories;
using Stockimulate.Persistence;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global

namespace Stockimulate
{
    sealed class Startup
    {
        readonly IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            _configuration = builder.Build();
            Constants.ConnectionString = _configuration.GetConnectionString("MS_TableConnectionString");
            Constants.PusherAppId = _configuration.GetConnectionString("Pusher_AppId");
            Constants.PusherSecret = _configuration.GetConnectionString("Pusher_Secret");
            Constants.PusherKey = _configuration.GetConnectionString("Pusher_Key");
            Constants.PusherCluster = _configuration.GetConnectionString("Pusher_Cluster");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddSingleton<ISimulator, Simulator>()
                .AddScoped<ILoginRepository, LoginRepository>()
                .AddScoped<ISecurityRepository, SecurityRepository>()
                .AddScoped<ITeamRepository, TeamRepository>()
                .AddScoped<ITradeRepository, TradeRepository>()
                .AddScoped<ITraderRepository, TraderRepository>()
                .AddScoped<ITradingDayRepository, TradingDayRepository>()
                .AddDbContext<StockimulateContext>(options => options.UseSqlServer(Constants.ConnectionString))
                    .AddDistributedMemoryCache() // Adds a default in-memory implementation of IDistributedCache.
                    .AddSession(options =>
                    {
                        // Set a short timeout for easy testing.
                        options.IdleTimeout = TimeSpan.FromDays(1);
                        options.Cookie.HttpOnly = true;
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(_configuration.GetSection("Logging"))
                         .AddDebug();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage()
                   .UseBrowserLink();
            else
                app.UseExceptionHandler("/Main/Error");

            app.UseStaticFiles()
               .UseSession()
               .UseMvc(routes => routes.MapRoute(
                "default",
                "{controller=NavigationLayout}/{action=Home}/{id?}"));
        }
    }
}
