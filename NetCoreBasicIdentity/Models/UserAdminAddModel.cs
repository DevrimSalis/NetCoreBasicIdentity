using System.ComponentModel.DataAnnotations;

namespace NetCoreBasicIdentity.Models
{
    public class UserAdminAddModel
    {
        [Required(ErrorMessage = "Kullanıcı adı boş geçilemez")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Eposta adresi boş geçilemez.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Cinsiyet alanı boş geçilemez")]
        public string Gender { get; set; }
    }
}