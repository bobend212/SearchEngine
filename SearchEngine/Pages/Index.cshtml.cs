using EuropePMC;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SearchEngine.Pages
{
    public class IndexModel : PageModel
    {
        private SearchService searchService = ApplicationContext.GetSearchService();
        public IList<result> Publications { get; set; }
        public int TotalCount { get; set; }

        public string SearchQuery { get; set; }

        public async Task OnGetAsync()
        {
            ApplicationContext.Reset();
            searchService = ApplicationContext.GetSearchService();
            await ShowPublications();
        }

        public async Task OnPostSearchPublication(string search)
        {
            if (String.IsNullOrWhiteSpace(search))
            {
                ApplicationContext.Reset();
                searchService = ApplicationContext.GetSearchService();
                await ShowPublications();
            }
            else
            {
                await ShowSpecificPublications(search);
                SearchQuery = $"\"{search}\"";
            }
        }

        public async Task OnPostNextPage()
        {
            string currentCursorMark = searchService.CurrentCursorMark();
            string currentSearch = searchService.CurrentSearch();

            if (currentCursorMark == "*")
            {
                Publications = await searchService.LoadSpecificPublications(currentSearch, searchService.GetNextCursorMark());
                TotalCount = searchService.CountPublications();
                return;
            }

            Publications = await searchService.LoadSpecificPublications(currentSearch, currentCursorMark);
            TotalCount = searchService.CountPublications();
        }

        private async Task ShowPublications()
        {
            Publications = await searchService.LoadPublications();
            TotalCount = searchService.CountPublications();
        }

        private Task ShowSpecificPublications(string searchText)
        {
            Publications = searchService.LoadSpecificPublications(searchText).Result;
            TotalCount = searchService.CountPublications();
            return Task.CompletedTask;
        }
    }
}