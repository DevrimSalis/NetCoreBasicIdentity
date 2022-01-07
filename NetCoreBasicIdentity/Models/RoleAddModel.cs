using System.ComponentModel.DataAnnotations;

namespace NetCoreBasicIdentity.Models
{
    public class RoleAddModel
    {
        [Required(ErrorMessage = "İsim alanı boş geçilemez.")]
        public string Name { get; set; }
    }
}