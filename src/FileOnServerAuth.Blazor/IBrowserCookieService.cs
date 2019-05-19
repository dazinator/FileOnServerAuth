using System.Threading.Tasks;

namespace Dazinator.FileOnServerAuth.Blazor
{
    public interface IBrowserCookieService
    {
        Task<string> GetCookie(string name);
    }
}