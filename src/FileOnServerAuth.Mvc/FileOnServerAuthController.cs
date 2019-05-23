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
    public class FileOnServerAuthController : Controller
    {

        private readonly ILogger<FileOnServerAuthController> _logger;
        private readonly IClaimsPrincipalFactory _principalFactory;
       // private readonly IOptions<AuthCodeOptions> _options;
        private readonly IOptions<FileOnServerAuthenticationOptions> _authCodePolicy;
        private readonly IAuthorizationService _authService;
        private readonly SystemAuthCodeProvider _authCodeProvider;

        public FileOnServerAuthController(
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

        // [Authorize(AuthenticationSchemes = AuthCodeOptions.DefaultAuthenticationSchemeName)]
        public async Task<IActionResult> Index()
        {

            var result = await _authService.AuthorizeAsync(this.User, _authCodePolicy.Value.Policy);
            if (!result.Succeeded)
            {
                return Challenge(_authCodePolicy.Value.AuthenticationScheme);
            }

            // If already authenticated with system authentication, redirect to home page.
            var values = new { Controller = "", Action = "" };
            return RedirectToRoute("default", values);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["AuthCodeFilePath"] = _authCodeProvider.PhysicalFilePath;
            var model = new AuthenticateViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm]AuthenticateViewModel authItem, string returnUrl)
        {
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
                        return RedirectToLocal(returnUrl);
                    }
                }

            }

            ModelState.AddModelError("Code", "Invalid code.");
            var newModel = new AuthenticateViewModel();
            ViewData["AuthCodeFilePath"] = _authCodeProvider.PhysicalFilePath;
            ViewData["ReturnUrl"] = returnUrl;
            return View(authItem);
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
