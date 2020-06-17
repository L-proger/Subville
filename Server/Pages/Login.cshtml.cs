using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Server.Pages
{
    public class LoginModel : PageModel
    {
        public IActionResult OnGet()
        {
            
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("~/Index");
            }
            else
            {
                return Page();
            }
            // if(AuthAppBuilderExtensions())
           // 
           //
        }
    }
}