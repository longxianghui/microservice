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
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })

                .AddIdentityServerAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme,
                //jwt settings
                o =>
                {
                    o.Authority = "http://identity";
                    o.Audience = "api1";
                    o.BackchannelHttpHandler = handler;//use discovery service
                    o.RequireHttpsMetadata = false;//dev not use https
                },
                //openId connet settings
                x =>
                {
                    x.Authority = "http://identity";
                    x.ClientId = "api1";
                    x.ClientSecret = "fsdafasdfasdsdfasecret";
                    x.DiscoveryHttpHandler = handler;//use discovery service
                    x.DiscoveryPolicy = new DiscoveryPolicy()
                    {
                        ValidateIssuerName = false,
                        ValidateEndpoints = false,
                        RequireHttps = false
                    };
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