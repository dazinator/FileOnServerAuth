using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text;

namespace Dazinator.FileOnServerAuth
{
    public class SystemAuthCodeProvider
    {

        AuthCodeOptions _options;
        private readonly PhysicalFileProvider _fileProvider;

        public string PhysicalFilePath { get; }

        public SystemAuthCodeProvider(
            IOptions<AuthCodeOptions> authCodeOptions,
            PhysicalFileProvider fileProvider
            )
        {

            _options = authCodeOptions.Value;
            _fileProvider = fileProvider;

            var filePath = _options.AuthCodeFilePath.Replace('/', '\\').TrimStart(new char[] { '\\' });
            var fullPath = System.IO.Path.Combine(_fileProvider.Root, filePath);
            PhysicalFilePath = fullPath;

            CreateAuthCodeFileIfNotExists();


        }



        /// <summary>
        /// Creates an auth code file, if the file doesn't already exist.
        /// </summary>
        /// <param name="file"></param>
        public void CreateAuthCodeFileIfNotExists()
        {
            var fileInfo = _fileProvider.GetFileInfo(_options.AuthCodeFilePath);
            if (!fileInfo.Exists)
            {
                var authCodeBuilder = new StringBuilder(_options.Length);
                var random = new Random();
                for (int i = 0; i < _options.Length; i++)
                {
                    authCodeBuilder.Append(GenerateLetter(random));
                }

                System.IO.File.WriteAllText(this.PhysicalFilePath, authCodeBuilder.ToString());
            }
        }


        public char GenerateLetter(Random random)
        {
            int number = random.Next(0, 26);
            char letter = (char)('a' + number);
            return letter;
        }

        public bool CheckIsValidCode(string code)
        {
            var fileInfo = _fileProvider.GetFileInfo(_options.AuthCodeFilePath);

            //  var file = _environment.ContentRootFileProvider.GetFileInfo(_options.AuthCodeFilePath);
            if (fileInfo.Exists)
            {
                using (var reader = new StreamReader(fileInfo.CreateReadStream()))
                {
                    var serverCode = reader.ReadToEnd();
                    return serverCode == code;
                }
            }
            return false;
        }
    }
}
