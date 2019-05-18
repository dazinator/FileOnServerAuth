namespace Dazinator.FileOnServerAuth
{
    public class AuthCodeOptions
    {
        public string AuthCodeFilePath { get; set; }
        public int Length { get; set; } = 10;
        public string PhysicalRootPath { get; set; }
    }
}