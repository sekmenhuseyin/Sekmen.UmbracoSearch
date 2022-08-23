using Umbraco.Cms.Core.Models.PublishedContent;

namespace Sekmen.UmbracoSearch.Models;

public class SearchModel : PublishedContentWrapped
{
    public SearchModel(IPublishedContent content, IPublishedValueFallback publishedValueFallback,
        string query, long totalResults, IEnumerable<SearchResultItem> searchResults, string docTypeToSearch)
        : base(content, publishedValueFallback)
    {
        Query = query;
        TotalResults = totalResults;
        SearchResults = searchResults;
        DocTypeToSearch = docTypeToSearch;
    }

    public string Query { get; }
    public long TotalResults { get; }
    public IEnumerable<SearchResultItem> SearchResults { get; }
    public string DocTypeToSearch { get; }
}