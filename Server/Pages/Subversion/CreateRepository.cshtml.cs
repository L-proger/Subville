using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Server.Pages.Subversion
{
    public class CreateRepositoryModel : PageModel
    {
        public string Message;
        public ISubvilleApiController Subversion { get; private set; }

        public CreateRepositoryModel(ISubvilleApiController subversionApi) {
            Subversion = subversionApi;
        }
        //
        public void OnGet()
        {
            System.Console.WriteLine($"Page get");
        }

        public IActionResult OnPostCreate(int templateId, string name) {
            System.Console.WriteLine ($"Form Posted: templateId={templateId}, repo name: {name}");
            try {
                (Subversion as SubvilleApiImpl).Subversion.CreateRepository(name, templateId == 0);
                return Redirect("/Subversion/RepositoryBrowser/" + name);
            }
            catch(Exception ex) {
                Message = ex.Message;
                return new PageResult();
            }
           

          

         
        }
    }
}
