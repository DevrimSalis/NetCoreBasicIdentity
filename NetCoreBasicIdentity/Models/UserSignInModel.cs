using System.ComponentModel.DataAnnotations;

namespace NetCoreBasicIdentity.Models
{
    public class UserSignInModel
    {
        [Required(ErrorMessage = "Kullanıcı adı boş geçilemez.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Parola boş geçilemez.")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
