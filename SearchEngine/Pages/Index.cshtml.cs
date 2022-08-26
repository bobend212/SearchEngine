using EuropePMC;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SearchEngine.Pages
{
    public class IndexModel : PageModel
    {
        private EuropePMC.WSCitationImpl service = new WSCitationImplClient();

        public IList<result> Collection { get; set; }
        public int TotalCount { get; set; }
        //public string NextPageString { get; set; }
        //public string SearchText { get; set; }

        private searchPublicationsRequest request;

        static string qString = "SRC:*";
        static string qMark = "*";

        public IndexModel()
        {
            request = new searchPublicationsRequest();
            request.queryString = qString;
            request.cursorMark = qMark;
        }

        private async Task LoadPublications(string searchText)
        {
            searchPublicationsResponse1 response = await service.searchPublicationsAsync(request);

            //NextPageString = response.@return.nextCursorMark;
            //SearchText = searchText;

            qString = searchText;


            //searchPublicationsResponse1 response2 = await service.searchPublicationsAsync(new searchPublicationsRequest()
            //{
            //    queryString = qString,
            //    cursorMark = qMark
            //});

            qMark = response.@return.nextCursorMark;

            Collection = response.@return.resultList;
            TotalCount = response.@return.hitCount;
        }

        public async Task OnGetAsync()
        {
            //await LoadPublications("SRC:*");

            request.queryString = "SRC:*";
            request.cursorMark = "*";

            searchPublicationsResponse1 response = await service.searchPublicationsAsync(request);

            request.queryString = "SRC:*";
            request.cursorMark = qMark;

            //NextPageString = response.@return.nextCursorMark;
            //SearchText = searchText;

            //searchPublicationsResponse1 response2 = await service.searchPublicationsAsync(new searchPublicationsRequest()
            //{
            //    queryString = "SRC:*",
            //    cursorMark = "*"
            //});

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

            //NextPageString = response.@return.nextCursorMark;
            //SearchText = searchText;

            //qString = qString;
            qMark = response.@return.nextCursorMark;

            //searchPublicationsResponse1 response2 = await service.searchPublicationsAsync(new searchPublicationsRequest()
            //{
            //    queryString = qString,
            //    cursorMark = qMark
            //});

            Collection = response.@return.resultList;
            TotalCount = response.@return.hitCount;
        }
    }
}