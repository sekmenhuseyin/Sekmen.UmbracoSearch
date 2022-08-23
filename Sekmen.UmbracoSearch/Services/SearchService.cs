using Examine;
using Examine.Search;
using Sekmen.UmbracoSearch.Extensions;
using Sekmen.UmbracoSearch.Models;
using System.Diagnostics;
using System.Globalization;
using Umbraco.Cms.Core.Web;

namespace Sekmen.UmbracoSearch.Services;

public interface ISearchService
{
    IEnumerable<SearchResultItem> GetContentSearchResults(string searchTerm, string contentType, out long totalItemCount);
}

public class SearchService : ISearchService
{
    private readonly IExamineManager _examineManager;
    private readonly IUmbracoContextAccessor _contextAccessor;

    public SearchService(IExamineManager examineManager, IUmbracoContextAccessor contextAccessor)
    {
        _examineManager = examineManager;
        _contextAccessor = contextAccessor;
    }

    public IEnumerable<SearchResultItem> GetContentSearchResults(string searchTerm, string contentType, out long totalItemCount)
    {
        totalItemCount = 0;
        var items = new List<SearchResultItem>();
        if (!_contextAccessor.TryGetUmbracoContext(out var umbracoContext))
            return items;


        var pageOfResults = GetSearchResults(searchTerm, contentType, out totalItemCount);
        foreach (var item in pageOfResults)
        {

            var page = umbracoContext.Content?.GetById(int.Parse(item.Id));
            if (page != null)
            {
                items.Add(new SearchResultItem
                {
                    PublishedItem = page,
                    Score = item.Score
                });
            }

            var pageMedia = umbracoContext.Media?.GetById(int.Parse(item.Id));
            if (pageMedia != null)
            {
                items.Add(new SearchResultItem
                {
                    PublishedItem = pageMedia,
                    Score = item.Score
                });
            }

            //we got no content and no media, so we assume the item is a To-Do
            if (page == null && pageMedia == null)
            {
                items.Add(new SearchResultItem
                {
                    ToDo = new ToDoModel
                    {
                        Title = item.GetValues("title").FirstOrDefault() ?? "",
                        Id = int.Parse(item.GetValues("id").FirstOrDefault() ?? "")
                    },
                    Score = item.Score
                });
            }

        }
        return items;
    }

    private IEnumerable<ISearchResult> GetSearchResults(string searchTerm, string contentType, out long totalItemCount)
    {
        totalItemCount = 0;
        searchTerm = searchTerm.MakeSearchQuerySafe();
        if (string.IsNullOrEmpty(searchTerm))
        {
            return Array.Empty<ISearchResult>();
        }

        //get examine external index: only get published content data
        if (!_examineManager.TryGetSearcher("MultiSearcher", out var multiSearcher))
            return Enumerable.Empty<ISearchResult>();

        //get searcher & create query
        var criteria = multiSearcher.CreateQuery(null, BooleanOperation.Or);

        //constants
        var fieldToSearchLang = "contents" + "_" + CultureInfo.CurrentCulture.ToString().ToLower();
        var fieldToSearchInvariant = "contents";
        var hideFromNavigation = "umbracoNaviHide";
        var pdfTextContent = "fileTextContent";
        var todoTitle = "title";
        var fieldsToSearch = new[] { fieldToSearchLang, fieldToSearchInvariant, pdfTextContent, todoTitle };

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
        if (contentType != "All")
        {
            examineQuery.And().Field("__NodeTypeAlias", contentType);
        }
        examineQuery.OrderByDescending(new SortableField("publishingDate", SortType.Long));

        //execute query
        Debug.WriteLine(criteria.ToString());
        var results = examineQuery.Execute();
        totalItemCount = results.TotalItemCount;

        //return results
        return results.Any()
            ? results
            : Enumerable.Empty<ISearchResult>();
    }
}