using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColorCode;
using ColorCode.Common;
using ColorCode.Styling;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Server.Pages.Subversion
{
   
    public class RepositoryBrowserModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string RepositoryName {
            get;
            set;
        }
        [BindProperty(SupportsGet = true)]
        public string RepositorySubpath {
            get;
            set;
        }

        public Integrations.Subversion.SvnList Data {
            get;
            set;
        }


        public string Content
        {
            get;
            private set;
        }

        public int LinesCount
        {
            get
            {
                return Content.Split('\n').Length;
            }
        }


        public string FormattedContent {
            get
            {
                var csharpstring = Content;
                var formatter = new HtmlFormatter();
             //   formatter.Styles[ScopeName.PlainText].Background = "#fffaf6fa";


                var html = formatter.GetHtmlString(csharpstring, Languages.CSharp);
                return html;
            }
     
        }

        public SubvilleApiImpl Api { get; private set; }
        public RepositoryBrowserModel(ISubvilleApiController subversionApi) {
            Api = (subversionApi as SubvilleApiImpl);
        }


        public string GetSubpath(string entryName)
        {
            var p = Request.Path;
            p = p.Add(new PathString($"/{entryName}"));
            return p;
        }

        public List<string> PathItems()
        {
            List<string> result = new List<string>();
            result.Add(RepositoryName);
            if (RepositorySubpath != null)
            {
                result.AddRange(RepositorySubpath.Split('/'));
            }
            
            return result;
        }


        public void OnGet()
        {
            try
            {
                Content = Api.Subversion.Cat(RepositoryName, RepositorySubpath);
            }
            catch (Exception ex)
            {

            }

            if (Content == null)
            { 
                Data = Api.Subversion.List(RepositoryName, RepositorySubpath);
            }
        }
    }
}
