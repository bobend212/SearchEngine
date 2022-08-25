using EuropePMC;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SearchEngine.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private EuropePMC.WSCitationImpl service = new WSCitationImplClient();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IList<result> Collection { get; set; }
        public int TotalCount { get; set; }

        public async Task OnGetAsync(string searchString)
        {
            await LoadPublications("SRC:*");
        }
        public async Task OnPostSearchPublication(string test2)
        {
            if (!String.IsNullOrWhiteSpace(test2))
            {
                await LoadPublications(test2);
            }
            else
            {
                await LoadPublications("SRC:*");
            }
        }

        private async Task LoadPublications(string searchText)
        {
            searchPublicationsResponse1 test = await service.searchPublicationsAsync(new searchPublicationsRequest()
            {
                queryString = searchText,
                cursorMark = "*"
            });

            Collection = test.@return.resultList;
            TotalCount = test.@return.hitCount;
        }
    }
}