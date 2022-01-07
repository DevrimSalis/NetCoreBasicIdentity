using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using NetCoreBasicIdentity.Entities;

namespace NetCoreBasicIdentity.TagHelpers
{
    [HtmlTargetElement("getUserInfo")]
    public class GetUserInfo:TagHelper
    {
        public int UserId { get; set; }   
        private readonly UserManager<AppUser> _userManager;

        public GetUserInfo(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var html = "";
            var user = await _userManager.Users.SingleOrDefaultAsync(u=>u.Id == UserId);
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                html += role + " ";
            }
            output.Content.SetHtmlContent(html);
        }
    }
}