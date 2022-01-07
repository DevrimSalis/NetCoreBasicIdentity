using Microsoft.AspNetCore.Identity;

namespace NetCoreBasicIdentity.SpecialDescriber
{
    public class CustomDescriber : IdentityErrorDescriber
    {
        public override IdentityError PasswordTooShort(int length)
        {
            return new()
            {
                Code = "PasswordTooShord",
                Description = $"Parola minimum{length} karakter olmalıdır."
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new()
            {
                Code = "PasswordRequiresNonAlphanumeric",
                Description = "Parolada en az bir özel karakter bulunmalıdır."
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new()
            {
                Code = "DuplicateUserName",
                Description = $"{userName} istemde tanımlı. Lütfen başka bir kullanıcı adı seçiniz."
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new()
            {
                Code = "PasswordRequiresLower",
                Description = "Paroladan en az bir küçük harf bulunmalıdır."
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new()
            {
                Code = "PasswordRequiresUpper",
                Description = "Paroladan en az bir büyük harf bulunmalıdır."
            };
        }
    }
}