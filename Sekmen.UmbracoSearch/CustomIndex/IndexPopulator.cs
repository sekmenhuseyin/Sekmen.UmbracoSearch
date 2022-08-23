using Examine;
using Newtonsoft.Json;
using Sekmen.UmbracoSearch.Models;
using System.Net;
using Umbraco.Cms.Infrastructure.Examine;

namespace Sekmen.UmbracoSearch.CustomIndex;

public class TodoIndexPopulator : IndexPopulator
{
    private readonly TodoValueSetBuilder _todoValueSetBuilder;
    public TodoIndexPopulator(TodoValueSetBuilder productValueSetBuilder)
    {
        _todoValueSetBuilder = productValueSetBuilder;
        //We're telling this populator that it's responsible for populating only our index
        RegisterIndex("TodoIndex");
    }
    protected override void PopulateIndexes(IReadOnlyList<IIndex> indexes)
    {
        using var httpClient = new WebClient();
        var jsonData = httpClient.DownloadString("https://jsonplaceholder.typicode.com/todos/");
        var data = JsonConvert.DeserializeObject<IEnumerable<ToDoModel>>(jsonData);

        if (data == null)
            return;

        foreach (var item in indexes)
        {
            item.IndexItems(_todoValueSetBuilder.GetValueSets(data.ToArray()));
        }
    }
}