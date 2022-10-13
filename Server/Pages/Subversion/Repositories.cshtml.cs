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

        public class RepoListData {
            public string Name { get; set; }
            public string URL { get; set; }
        }

        public class RepoListPage {
            public List<RepoListData> Data { get; set; }
            public int Page { get; set; }
            public int PageSize { get; set; }
            public int PageCount { get; set; }
        }

        public JsonResult OnGetRepositoriesList(string filter, int pageIndex, int pageSize) {
            var repoNames = (Subversion as SubvilleApiImpl).Subversion.GetRepositories();

            if (!string.IsNullOrWhiteSpace(filter)) {
                repoNames = repoNames.Where(x => x.ToLowerInvariant().Contains(filter.ToLowerInvariant())).ToArray();
            }

            RepoListPage result = new RepoListPage();
            result.PageCount = (repoNames.Length + (pageSize - 1)) / pageSize;

            if(pageIndex >= result.PageCount) {
                pageIndex = result.PageCount - 1;
            }

            result.Page = pageIndex;
            result.PageSize = pageSize;
            result.Data = repoNames.Skip(pageIndex * pageSize).Take(pageSize).Select(x => new RepoListData() { Name = x, URL = $"/Subversion/RepositoryBrowser/{x}" }).ToList();

            return new JsonResult(result);
        }
    }
}
