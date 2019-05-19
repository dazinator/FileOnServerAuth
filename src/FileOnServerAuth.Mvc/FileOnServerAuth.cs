using Dazinator.FileOnServerAuth.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Dazinator.FileOnServerAuth.Mvc
{
    [Route("api/[controller]")]
    public class FileOnServerAuth : Controller
    {

        private readonly ILogger<FileOnServerAuthController> _logger;
        private readonly IClaimsPrincipalFactory _principalFactory;
        // private readonly IOptions<AuthCodeOptions> _options;
        private readonly IOptions<FileOnServerAuthenticationOptions> _authCodePolicy;
        private readonly IAuthorizationService _authService;
        private readonly SystemAuthCodeProvider _authCodeProvider;

        public FileOnServerAuth(
            ILogger<FileOnServerAuthController> logger,
            IClaimsPrincipalFactory prinipalFactory,
            IAuthorizationService authService,
            SystemAuthCodeProvider authCodeProvider,
             IOptions<FileOnServerAuthenticationOptions> policy
            )
        {
            _logger = logger;
            _principalFactory = prinipalFactory;
            _authService = authService;
            _authCodeProvider = authCodeProvider;
            _authCodePolicy = policy;
        }     
       

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AuthenticateViewModel authItem)
        {
            var returnUrl = "/";
            if (ModelState.IsValid)
            {
                // compare the code.
                if (!string.IsNullOrWhiteSpace(authItem.Code))
                {
                    bool isCodeValid = _authCodeProvider.CheckIsValidCode(authItem.Code);
                    if (isCodeValid)
                    {
                        var sysIdentity = _principalFactory.GetIdentity();

                        // need to set a temporary system login.
                        var authProps = new AuthenticationProperties();
                        authProps.ExpiresUtc = DateTime.UtcNow.Add(_authCodePolicy.Value.LoginExpiresAfter);

                        await HttpContext.SignInAsync(_authCodePolicy.Value.AuthenticationScheme, sysIdentity, authProps);
                     //   return Json(new { Status = "Success" };
                        return RedirectToLocal(returnUrl);
                    }
                }

            }

            ModelState.AddModelError("Code", "Invalid code.");
            var newModel = new AuthenticateViewModel();
            ViewData["AuthCodeFilePath"] = _authCodeProvider.PhysicalFilePath;
            ViewData["ReturnUrl"] = returnUrl;
            return Json(authItem);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToRoute("default");
                //  return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

    }
}
