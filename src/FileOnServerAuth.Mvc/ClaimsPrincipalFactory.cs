using Dazinator.FileOnServerAuth.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Dazinator.FileOnServerAuth
{
    public class ClaimsPrincipalFactory : IClaimsPrincipalFactory
    {
        private readonly FileOnServerAuthenticationOptions _options;

        public ClaimsPrincipalFactory(IOptions<FileOnServerAuthenticationOptions> authCodeOptions)           
        {
            _options = authCodeOptions.Value;     
        }
        
        public ClaimsPrincipal GetIdentity()
        {
            var sysIdentity = new ClaimsIdentity(_options.AuthenticationType);
            if(!string.IsNullOrWhiteSpace(_options.GrantRoleClaim))
            {
                sysIdentity.AddClaim(new Claim(ClaimTypes.Role, _options.GrantRoleClaim));
            }
            if (!string.IsNullOrWhiteSpace(_options.GrantNameClaim))
            {
                sysIdentity.AddClaim(new Claim(ClaimTypes.Name, _options.GrantNameClaim));
            }

            var claimsPrincipal = new ClaimsPrincipal(sysIdentity);
            return claimsPrincipal;          
        }
    }
}
