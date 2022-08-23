using Examine.Lucene;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Examine;

namespace Sekmen.UmbracoSearch.CustomIndex;

public class CustomToDoIndex : UmbracoExamineIndex
{
    public CustomToDoIndex(
        ILoggerFactory loggerFactory,
        string name,
        IOptionsMonitor<LuceneDirectoryIndexOptions> indexOptions,
        Umbraco.Cms.Core.Hosting.IHostingEnvironment hostingEnvironment,
        IRuntimeState runtimeState)
        : base(loggerFactory, name, indexOptions, hostingEnvironment, runtimeState)
    {
    }

    public const string StaticName = "TodoIndex";
}