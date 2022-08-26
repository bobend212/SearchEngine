using EuropePMC;

public class SearchService
{
    private EuropePMC.WSCitationImpl service = new WSCitationImplClient();
    private string currentCursorMark = "*";
    private string currentSearch = "SRC:*";
    private int totalCount = 0;

    public SearchService()
    {
        currentCursorMark = "*";
        currentSearch = "SRC:*";
    }

    public string CurrentCursorMark() => currentCursorMark;

    public int CountPublications() => totalCount;

    public string CurrentSearch() => currentSearch;

    public async Task<IList<result>> LoadPublications()
    {
        searchPublicationsRequest request = new searchPublicationsRequest
        {
            queryString = currentSearch,
            cursorMark = currentCursorMark
        };

        searchPublicationsResponse1 response = await service.searchPublicationsAsync(request);

        currentCursorMark = response.@return.nextCursorMark;
        totalCount = response.@return.hitCount;

        return new List<result>(response.@return.resultList);
    }

    public async Task<IList<result>> LoadSpecificPublications(string queryString, string cursorMark = "*")
    {
        currentSearch = queryString;
        searchPublicationsRequest request = new searchPublicationsRequest
        {
            queryString = queryString,
            cursorMark = cursorMark
        };
        searchPublicationsResponse1 response = await service.searchPublicationsAsync(request);
        currentCursorMark = response.@return.nextCursorMark;
        totalCount = response.@return.hitCount;

        return new List<result>(response.@return.resultList);
    }

    public string GetNextCursorMark()
    {
        searchPublicationsRequest request = new searchPublicationsRequest
        {
            queryString = currentSearch,
            cursorMark = currentCursorMark
        };
        searchPublicationsResponse1 response = service.searchPublicationsAsync(request).Result;
        currentCursorMark = response.@return.nextCursorMark;
        return currentCursorMark;
    }
}