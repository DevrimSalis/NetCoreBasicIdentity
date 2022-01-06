using System.ComponentModel.DataAnnotations;

namespace NetCoreBasicIdentity.Models
{
    public class UserCreateModel
    {
        [Required(ErrorMessage = "Kullanıcı adı boş geçilemez")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Eposta adresi boş geçilemez")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Parola boş geçilemez.")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Parola alanları aynı değil.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Cinsiyet adı boş geçilemez")]
        public string Gender { get; set; }
    }
}
