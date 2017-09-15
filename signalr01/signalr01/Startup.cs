using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.SignalR;
using signalr01.Hubs;

namespace signalr01
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
            services.AddMvc();

            //>Add permission enable cross-origin requests (CORS) from angular
            services.AddCors(options =>
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                        builder.AllowCredentials();
                    })
                );
            //<Add permission

            //>Add SignalR
            services.AddSignalR(options => {});
            //<AddSignalR
            
            services.AddAuthentication().AddCookie();

            //>AddSingleton
            services.AddSingleton(typeof(DefaultHubLifetimeManager<>), typeof(DefaultHubLifetimeManager<>));
            services.AddSingleton(typeof(HubLifetimeManager<>), typeof(DefaultPresenceHublifetimeManager<>));
            services.AddSingleton(typeof(IUserTracker<>), typeof(InMemoryUserTracker<>));
            //<AddSingleton
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseWebSockets();

            //>UseSignalR
            app.UseSignalR(config =>
            {
                config.MapHub<Chat>("chat");
            });
            //<UseSignalR

            app.UseCors("AllowAll");
            //UseCors allow origin for test signalR by fontend other Origins
            app.UseCors(config => config.WithOrigins("http://localhost:58141")
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
            //<UseSignalR

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
