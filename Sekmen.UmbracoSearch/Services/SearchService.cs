using Examine;
using Examine.Search;
using Sekmen.UmbracoSearch.Extensions;
using Sekmen.UmbracoSearch.Models;
using System.Diagnostics;
using System.Globalization;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Examine;

namespace Sekmen.UmbracoSearch.Services
{
    public class SearchService : ISearchService
    {
        private readonly IExamineManager _examineManager;
        private readonly IUmbracoContextAccessor _contextAccessor;

        public SearchService(IExamineManager examineManager, IUmbracoContextAccessor contextAccessor)
        {
            _examineManager = examineManager;
            _contextAccessor = contextAccessor;
        }

        public IEnumerable<SearchResultItem> GetContentSearchResults(string searchTerm, out long totalItemCount)
        {
            var pageOfResults = GetSearchResults(searchTerm, out totalItemCount);
            var items = new List<SearchResultItem>();
            if (pageOfResults == null || !pageOfResults.Any())
                return items;

            foreach (var item in pageOfResults)
            {
                if (!_contextAccessor.TryGetUmbracoContext(out var umbracoContext))
                    continue;

                var page = umbracoContext.Content?.GetById(int.Parse(item.Id));
                if (page != null)
                {
                    items.Add(new SearchResultItem()
                    {
                        PublishedItem = page,
                        Score = item.Score
                    });
                }
            }
            return items;
        }

        public IEnumerable<ISearchResult> GetSearchResults(string searchTerm, out long totalItemCount)
        {
            totalItemCount = 0;
            searchTerm = searchTerm.MakeSearchQuerySafe();
            if (string.IsNullOrEmpty(searchTerm))
            {
                return Array.Empty<ISearchResult>();
            }

            //get examine external index: only get published content data
            if (!_examineManager.TryGetIndex(Constants.UmbracoIndexes.ExternalIndexName, out var index))
                return Enumerable.Empty<ISearchResult>();

            //get searcher & create query
            var searcher = index.Searcher;
            var criteria = searcher.CreateQuery(IndexTypes.Content);

            //constants
            var fieldToSearchLang = "contents" + "_" + CultureInfo.CurrentCulture.ToString().ToLower();
            var fieldToSearchInvariant = "contents";
            var hideFromNavigation = "umbracoNaviHide";
            var fieldsToSearch = new[] { fieldToSearchLang, fieldToSearchInvariant };

            //write query
            IBooleanOperation? examineQuery;
            if (searchTerm.Contains(" "))
            {
                var terms = searchTerm.Split(' ');
                examineQuery = criteria.GroupedOr(fieldsToSearch, terms);
            }
            else
            {
                examineQuery = criteria.GroupedOr(fieldsToSearch, searchTerm.MultipleCharacterWildcard());
            }
            examineQuery.Not().Field(hideFromNavigation, 1.ToString());


            //execute query
            var results = examineQuery.Execute();
            totalItemCount = results.TotalItemCount;

            //return results
            if (!results.Any())
                return Enumerable.Empty<ISearchResult>();

            Debug.WriteLine(criteria.ToString());
            return results;


        }
    }
}
