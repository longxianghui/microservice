using System.Threading.Tasks;
using IdentityServer4.Validation;

namespace IdentityServer
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            //调用用户中心的验证用户名密码接口
            string userId = "1";
            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new GrantValidationResult(null);
            }
            else
            {
                context.Result = new GrantValidationResult(userId, "password");
            }
            return Task.FromResult(0);
        }
    }
}