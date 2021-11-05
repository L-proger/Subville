using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ColorCode;
using ColorCode.Common;
using ColorCode.Styling;
using Markdig;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
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

        public static string MarkdownToHtml(string mdString)
        {
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            return Markdown.ToHtml(mdString, pipeline);
        }


        public bool HasChildReadmeFile() {
            return Data != null && Data.Entry != null && Data.Entry.Any(v => v.Name.ToLowerInvariant() == "readme.md" && v.Kind == "file");
        }

        public string GetChildReadmeFileFormatted() {

             var entry = Data.Entry.First(v => v.Name.ToLowerInvariant() == "readme.md" && v.Kind == "file");
             var str = Api.Subversion.Cat(RepositoryName, RepositorySubpath + "/" + entry.Name);
             return MarkdownToHtml(str);
        }


        public string Content
        {
            get;
            private set;
        }

        public int LinesCount => Content.Split('\n').Length;
        public int CurrentFileSize => Api.Subversion.GetFileSize(RepositoryName, RepositorySubpath);


        private IWebHostEnvironment Environment;

        //svnlook filesize  E:/Repositories/TestRepo1 /trunk/InspectorButton.cs
        public string FormattedContent {
            get
            {
                if (RepositorySubpath.EndsWith(".md"))
                {
                    return MarkdownToHtml(Content);
                }
                else
                {
                    var csharpstring = Content;
                    var formatter = new HtmlFormatter();
                    //   formatter.Styles[ScopeName.PlainText].Background = "#fffaf6fa";


                    var html = formatter.GetHtmlString(csharpstring, Languages.CSharp);
                    return html;
                }
              
            }
     
        }

        public SubvilleApiImpl Api { get; private set; }
        public RepositoryBrowserModel(ISubvilleApiController subversionApi, IWebHostEnvironment _environment) {
            Api = (subversionApi as SubvilleApiImpl);
            Environment = _environment;
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

        public string OcticonsSvgDirectory
        {
            get
            {
                return Path.Combine(this.Environment.WebRootPath, "lib", "primer", "octicons", "build", "svg");
            }
        }

        public HtmlString Octicon(string iconName)
        {
            var dir = OcticonsSvgDirectory;
            var path = Path.Combine(dir, iconName + ".svg");
            var content = System.IO.File.ReadAllText(path);

            content = content.Replace("<svg", "<svg class=\"octicon\"");

           
            return new HtmlString(content);
        }

        public void OnGet()
        {

           
            string wwwPath = OcticonsSvgDirectory;



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
