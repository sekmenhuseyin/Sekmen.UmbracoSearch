using Umbraco.Cms.Core.Models.PublishedContent;

namespace Sekmen.UmbracoSearch.Models
{
    public class SearchResultItem
    {
        public IPublishedContent PublishedItem { get; init; }
        public ToDoModel ToDo { get; init; }
        public float Score { get; init; }
    }
}
