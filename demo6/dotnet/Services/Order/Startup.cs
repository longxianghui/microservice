using System.Net.Http;
using IdentityModel.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pivotal.Discovery.Client;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

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
            services.AddAuthentication(x =>
                {
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
            .AddIdentityServerAuthentication(x =>
             {
                 x.ApiName = "api1";
                 x.ApiSecret = "secret";
                 x.Authority = "http://identity";
                 x.RequireHttpsMetadata = false;
                 x.JwtBackChannelHandler = handler;
                 x.IntrospectionDiscoveryHandler = handler;
                 x.IntrospectionBackChannelHandler = handler;
             });
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