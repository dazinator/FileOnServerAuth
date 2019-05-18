using Microsoft.Extensions.Options;
using System;

namespace Dazinator.FileOnServerAuth
{
    public class AuthCodeOptionsPostConfigureOptions : IPostConfigureOptions<AuthCodeOptions>
    {
        public void PostConfigure(string name, AuthCodeOptions options)
        {
            if (string.IsNullOrEmpty(options.AuthCodeFilePath))
            {
                throw new InvalidOperationException("AuthCodeFilePath must be provided in options");
            }

            if (options.Length <= 0)
            {
                throw new InvalidOperationException("Length greater than 0 must be provided in options");
            }
        }
    }
}
