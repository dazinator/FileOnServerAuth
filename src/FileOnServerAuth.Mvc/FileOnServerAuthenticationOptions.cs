using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System;

namespace Dazinator.FileOnServerAuth.Mvc
{
    public class FileOnServerAuthenticationOptions
    {

        public const string DefaultAuthenticationSchemeName = "FileOnServer";
        public const string DefaultAuthenticationType = DefaultAuthenticationSchemeName;
        public const string DefaultGrantRoleNameClaim = "System Administrator";
        public const string DefaultGrantNameClaim = "System Administrator";
        public const string DefaultDisplayName = "File On Server";

        public static TimeSpan DefaultLoginExpiresAfter = new TimeSpan(1, 0, 0);


        public FileOnServerAuthenticationOptions()
        {
            // _options = options;

        }

        public string GrantRoleClaim { get; set; } = DefaultGrantRoleNameClaim;
        public string GrantNameClaim { get; set; } = DefaultGrantNameClaim;
        internal string AuthenticationScheme { get; set; } = DefaultAuthenticationSchemeName;
        public string AuthenticationType { get; set; } = DefaultAuthenticationType;

        public TimeSpan LoginExpiresAfter { get; set; } = DefaultLoginExpiresAfter;

        public string GetAuthenticationScheme()
        {
            return AuthenticationScheme;
        }

        internal AuthorizationPolicy Policy { get; set; }

        public CookieAuthenticationOptions CookieOptions { get; set; }

        public AuthorizationPolicy GetPolicy()
        {
            return Policy;
        }
    }
}
