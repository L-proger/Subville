using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Attributes;
using Server.Integrations;

namespace Server.Pages.Subversion {
    [UnAuthorized]
    public class RepositoriesModel : PageModel
    {
        public ISubvilleApiController Subversion { get; private set; }
        public RepositoriesModel(ISubvilleApiController subversionApi, IWebHostEnvironment environment) {
            Subversion = subversionApi;
            Icons = new Octicons(environment);
        }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public string[] Data { get; set; }


        public Octicons Icons;

        public void OnGet()
        {
            var fullData = (Subversion as SubvilleApiImpl).Subversion.GetRepositories();
            Data = fullData.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToArray();
            Count = fullData.Length;
        }
    }
}
