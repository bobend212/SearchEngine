using EuropePMC;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SearchEngine.Pages
{
    public class IndexModel : PageModel
    {
        private EuropePMC.WSCitationImpl service = new WSCitationImplClient();

        public IList<result> Collection { get; set; }
        public int TotalCount { get; set; }

        private searchPublicationsRequest request;

        private static string qString = "SRC:*";
        private static string qMark = "*";

        public IndexModel()
        {
            request = new searchPublicationsRequest();
            request.queryString = qString;
            request.cursorMark = qMark;
        }

        private async Task LoadPublications(string searchText)
        {
            request.queryString = searchText;
            request.cursorMark = "*";

            searchPublicationsResponse1 response = await service.searchPublicationsAsync(request);

            qString = searchText;
            qMark = response.@return.nextCursorMark;

            Collection = response.@return.resultList;
            TotalCount = response.@return.hitCount;
        }

        public async Task OnGetAsync()
        {
            request.queryString = "SRC:*";
            request.cursorMark = "*";

            searchPublicationsResponse1 response = await service.searchPublicationsAsync(request);

            qMark = response.@return.nextCursorMark;
            qString = "SRC:*";

            Collection = response.@return.resultList;
            TotalCount = response.@return.hitCount;
        }

        public async Task OnPostSearchPublication(string search)
        {
            if (!String.IsNullOrWhiteSpace(search))
            {
                await LoadPublications(search);
            }
            else
            {
                await LoadPublications("SRC:*");
            }
        }

        public async Task OnPostNextPage()
        {
            searchPublicationsResponse1 response = await service.searchPublicationsAsync(request);

            qMark = response.@return.nextCursorMark;

            Collection = response.@return.resultList;
            TotalCount = response.@return.hitCount;
        }
    }
}