using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;

namespace Dazinator.FileOnServerAuth.Mvc
{
    public class AuthenticationOptionsPostConfigureOptions : IPostConfigureOptions<FileOnServerAuthenticationOptions>
    {
        public void PostConfigure(string name, FileOnServerAuthenticationOptions options)
        {
            if (string.IsNullOrEmpty(options.AuthenticationScheme))
            {
                throw new InvalidOperationException("AuthenticationScheme must be provided in options");
            }

            options.Policy = new AuthorizationPolicyBuilder(options.AuthenticationScheme)
               .RequireAuthenticatedUser()
               .Build();          
        }
    }
}
