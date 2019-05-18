using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dazinator.FileOnServerAuth
{
    public class AuthenticateViewModel
    {

        [DisplayName("Auth Code")]
        [Required]
        public string Code { get; set; }     

    }
}
