using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stockimulate.Core.Repositories;
using Stockimulate.Persistence;

namespace Stockimulate
{
    [SuppressMessage("ReSharper", "MemberCanBeInternal")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
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
            Constants.ConnectionString = Configuration.GetConnectionString("MS_TableConnectionString");
            Constants.PusherAppId = Configuration.GetConnectionString("Pusher_AppId");
            Constants.PusherSecret = Configuration.GetConnectionString("Pusher_Secret");
            Constants.PusherKey = Configuration.GetConnectionString("Pusher_Key");
            Constants.PusherCluster = Configuration.GetConnectionString("Pusher_Cluster");
        }

        public IConfigurationRoot Configuration { get; }

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
            loggerFactory.AddConsole(Configuration.GetSection("Logging"))
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
