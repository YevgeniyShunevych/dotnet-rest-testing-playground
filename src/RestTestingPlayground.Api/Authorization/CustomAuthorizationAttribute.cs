using Microsoft.AspNetCore.Mvc;

namespace RestTestingPlayground.Api.Authorization
{
    public class CustomAuthorizationAttribute : TypeFilterAttribute
    {
        public CustomAuthorizationAttribute(params string[] policies)
            : base(typeof(CustomAuthorizationService))
        {
            Arguments = new object[] { policies };
        }
    }
}
