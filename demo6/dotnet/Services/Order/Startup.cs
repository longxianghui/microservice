using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pivotal.Discovery.Client;
using IdentityServer4.AccessTokenValidation;

namespace Order
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
            services.AddDiscoveryClient(Configuration);
            var discoveryClient = services.BuildServiceProvider().GetService<IDiscoveryClient>();
            var handler = new DiscoveryHttpClientHandler(discoveryClient);
            services.AddAuthorization();
            services.AddAuthentication("Bearer")
                                .AddIdentityServerAuthentication(x =>
                                {
                                    x.ApiName = "api1";
                                    x.Authority = "http://identity";
                                    x.ApiSecret = "secret";
                                    x.RequireHttpsMetadata = false;
                                    x.JwtBackChannelHandler = handler;
                                }
                )
                ;
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseMvc();
            app.UseDiscoveryClient();
        }
    }
}