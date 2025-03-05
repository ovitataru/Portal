using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Portal.Pages.Account
{
    public class ProfileModel : PageModel
    {
        public void OnGet()
        {
        }

        public IEnumerable<System.Security.Claims.Claim> Claims
        {
            get => User?.Claims;
        }
    }
}
