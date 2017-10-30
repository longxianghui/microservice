using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Common;
using IdentityServer4.Validation;
using Pivotal.Discovery.Client;

namespace Identity
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly DiscoveryHttpClientHandler _handler;
        private const string UserApplicationName = "user";

        public ResourceOwnerPasswordValidator(IDiscoveryClient client)
        {
            _handler = new DiscoveryHttpClientHandler(client);
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            //调用用户中心的验证用户名密码接口
            var client = new HttpClient(_handler, false);
            var url = $"http://{UserApplicationName}/search?name={context.UserName}&password={context.Password}";
            var result = client.GetAsync(url).Result;
            if (result.IsSuccessStatusCode)
            {
                var user = result.Content.ReadAsObjectAsync<dynamic>().Result;
                context.Result = new GrantValidationResult(user.id.ToString(), "password");
            }
            else
            {
                context.Result = new GrantValidationResult(null);
            }
           
            return Task.FromResult(0);
        }
    }
}