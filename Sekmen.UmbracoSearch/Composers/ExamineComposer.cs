using Examine;
using Sekmen.UmbracoSearch.Components;
using Sekmen.UmbracoSearch.CustomIndex;
using Sekmen.UmbracoSearch.Extensions;
using Sekmen.UmbracoSearch.Services;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Infrastructure.Examine;
using UmbracoExamine.PDF;

namespace Sekmen.UmbracoSearch.Composers;

[ComposeAfter(typeof(ExaminePdfComposer))]
public class ExamineComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.AddSingleton<ISearchService, SearchService>();
        builder.Components().Append<ExamineComponents>();
        builder.Services.ConfigureOptions<ConfigureCustomFieldOptions>();

        //add CustomIndex
        builder.Services.AddSingleton<TodoValueSetBuilder>();
        builder.Services.AddSingleton<IIndexPopulator, TodoIndexPopulator>();
        builder.Services.AddExamineLuceneIndex<CustomToDoIndex, ConfigurationEnabledDirectoryFactory>("TodoIndex");

    }
}