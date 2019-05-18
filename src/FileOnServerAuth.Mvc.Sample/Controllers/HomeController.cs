using Dazinator.FileOnServerAuth;
using Dazinator.FileOnServerAuth.Mvc;
using FileOnServerAuth.Mvc.Sample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FileOnServerAuth.Mvc.Sample
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = "TestLocalFile")]
        public async Task<IActionResult> Privacy(
            [FromServices]IOptions<FileOnServerAuthenticationOptions> options,
            [FromServices]IAuthorizationService authService)
        {          
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
