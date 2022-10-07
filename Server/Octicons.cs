using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using System.IO;

namespace Server {
    public class Octicons {
        private IWebHostEnvironment _environment;
        public Octicons(IWebHostEnvironment model) {
            _environment = model;
        }

        public string OcticonsSvgDirectory {
            get {
                return Path.Combine(_environment.WebRootPath, "lib", "primer", "octicons", "build", "svg");
            }
        }

        public HtmlString GetIconHtml(string iconName) {
            var dir = OcticonsSvgDirectory;
            var path = Path.Combine(dir, iconName + ".svg");
            var content = System.IO.File.ReadAllText(path);
            content = content.Replace("<svg", "<svg class=\"octicon\"");
            return new HtmlString(content);
        }
    }
}
