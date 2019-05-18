using Dazinator.FileOnServerAuth;
using Dazinator.FileOnServerAuth.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FileOnServerAuthenticationExtensions
    {

        public const string LoginPathMvc = "/FileOnServerAuth/Login/";
        public static AuthenticationBuilder AddFileOnServerAuthenticationMvc(this AuthenticationBuilder builder,

            string authenticationScheme = FileOnServerAuthenticationOptions.DefaultAuthenticationSchemeName,
            string displayName = FileOnServerAuthenticationOptions.DefaultDisplayName,
            Action<FileOnServerAuthenticationOptions> configureOptions = null,
            Action<CookieAuthenticationOptions> configureCookieOptions = null)
        {
            builder.Services.AddSingleton<IClaimsPrincipalFactory, ClaimsPrincipalFactory>();
            builder.Services.ConfigureOptions<AuthenticationOptionsPostConfigureOptions>();

            builder.Services.Configure<FileOnServerAuthenticationOptions>((o)=> {
                o.AuthenticationScheme = authenticationScheme;
                configureOptions?.Invoke(o);
            });            
           
            builder.AddCookie(authenticationScheme, displayName, (cookieOptions) =>
            {
                cookieOptions.LoginPath = new PathString(LoginPathMvc);
                cookieOptions.ReturnUrlParameter = "returnUrl";               
                configureCookieOptions?.Invoke(cookieOptions);
            });        

            return builder;
            // return builder.<LocalFileAuthenticationOptions, LocalFileAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}
