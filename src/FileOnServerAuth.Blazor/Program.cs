using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Dazinator.FileOnServerAuth.Blazor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();


        }

        public static IWebAssemblyHostBuilder CreateHostBuilder(string[] args) =>
            BlazorWebAssemblyHost.CreateDefaultBuilder()
                .UseBlazorStartup<Startup>();
    }

    /// <summary>
    /// Service for reading the current cookie on the browser
    /// </summary>
    public class BrowserCookieService : IBrowserCookieService
    {
        private readonly IJSRuntime _interop;

        public BrowserCookieService(IJSRuntime interop)
        {
            _interop = interop;
        }

        public async Task<string> GetCookie(string name)
        {
            var result = await _interop.InvokeAsync<string>("getCookie", name);
            return result;
        }


    }
}
