using System.Net.Http;
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
            var urls = discoveryClient.GetInstances("identity");

            services.AddAuthorization();
            services.AddAuthentication(x => x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme)
                .AddOpenIdConnect(x =>
                {
                    x.Authority = "http://identity";
                    x.
                    
                    x.ConfigurationManager =new ConfigurationManager<OpenIdConnectConfiguration>("http://",
                        new OpenIdConnectConfigurationRetriever(),
                        new HttpDocumentRetriever(new HttpClient(handler, false)));
                })
                .AddJwtBearer(x =>
                {
                    x.Audience
                    x.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>("http://",
                        new OpenIdConnectConfigurationRetriever(),
                        new HttpDocumentRetriever(new HttpClient(handler, false)));
                });
//            services.AddAuthentication("Bearer")
//                                .AddIdentityServerAuthentication(x =>
//                                {
//                                    x.ApiName = "api1";
//                                    x.Authority = urls[0].Uri.AbsoluteUri;
//                                    x.ApiSecret = "secret";
//                                    x.RequireHttpsMetadata = false;
//                                    x.JwtBackChannelHandler = handler;
//                                    
//                                }
//                )
//                ;
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