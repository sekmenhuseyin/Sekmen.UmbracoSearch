using Umbraco.Cms.Core.Models.PublishedContent;

namespace Sekmen.UmbracoSearch.Models
{
    public class SearchModel : PublishedContentWrapped
    {
        public SearchModel(IPublishedContent content, IPublishedValueFallback publishedValueFallback)
            : base(content, publishedValueFallback)
        {
        }

        public string Query { get; set; }
        public long TotalResults { get; set; }
        public IEnumerable<SearchResultItem> SearchResults { get; set; }
    }
}
