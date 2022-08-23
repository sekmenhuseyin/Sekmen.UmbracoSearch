using Examine;
using Sekmen.UmbracoSearch.Models;

namespace Sekmen.UmbracoSearch.Services
{
    public interface ISearchService
    {
        IEnumerable<ISearchResult> GetSearchResults(string searchTerm, out long totalItemCount);
        IEnumerable<SearchResultItem> GetContentSearchResults(string searchTerm, out long totalItemCount);
    }
}
