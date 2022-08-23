using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Sekmen.UmbracoSearch.Models;
using Sekmen.UmbracoSearch.Services;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace Sekmen.UmbracoSearch.Controllers
{
    public class SearchPageController : RenderController
    {
        private readonly ISearchService _searchService;
        private readonly IVariationContextAccessor _variationContextAccessor;
        private readonly ServiceContext _serviceContext;

        public SearchPageController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor, ISearchService searchService, IVariationContextAccessor variationContextAccessor, ServiceContext serviceContext)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _searchService = searchService;
            _variationContextAccessor = variationContextAccessor;
            _serviceContext = serviceContext;
        }

        public IActionResult SearchPage(string query, string docTypeToSearch)
        {
            var searchResults = Enumerable.Empty<SearchResultItem>();
            long totalResultCount = 0;
            if (string.IsNullOrEmpty(docTypeToSearch))
                docTypeToSearch = "All";

            //query might be null if we navigate across different language versions of the page
            if (!string.IsNullOrEmpty(query))
            {
                searchResults = _searchService.GetContentSearchResults(query, docTypeToSearch, out var totalItemCount);
                totalResultCount = totalItemCount;

            }

            var searchPageModel = new SearchModel(CurrentPage,
                new PublishedValueFallback(_serviceContext, _variationContextAccessor))
            {
                Query = query,
                DocTypeToSearch = docTypeToSearch,
                SearchResults = searchResults,
                TotalResults = totalResultCount
            };
            return CurrentTemplate(searchPageModel);

        }
    }
}
