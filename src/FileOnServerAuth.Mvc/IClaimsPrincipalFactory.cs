using System.Security.Claims;

namespace Dazinator.FileOnServerAuth
{
    public interface IClaimsPrincipalFactory
    {
        ClaimsPrincipal GetIdentity();
    }
}