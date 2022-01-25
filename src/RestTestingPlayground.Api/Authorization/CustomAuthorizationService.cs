using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RestTestingPlayground.Api.Authorization
{
    public class CustomAuthorizationService : IAsyncAuthorizationFilter
    {
        private readonly string[] _policies;

        private readonly IAuthorizationService _authorizationService;

        public CustomAuthorizationService(string[] policies, IAuthorizationService authorizationService)
        {
            _policies = policies;
            _authorizationService = authorizationService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            foreach (string policy in _policies)
            {
                if (!await AuthorizePolicy(context, policy))
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
        }

        private async Task<bool> AuthorizePolicy(AuthorizationFilterContext context, string policy)
        {
            return (await _authorizationService.AuthorizeAsync(context.HttpContext.User, policy)).Succeeded;
        }
    }
}
