public static class ApplicationContext
{
    private static SearchService searchService = new SearchService();

    public static void Reset()
    {
        searchService = new SearchService();
    }

    public static SearchService GetSearchService()
    {
        return searchService;
    }
}