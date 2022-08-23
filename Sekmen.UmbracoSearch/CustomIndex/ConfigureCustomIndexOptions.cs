﻿using Examine;
using Examine.Lucene;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Util;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Configuration.Models;

namespace Sekmen.UmbracoSearch.CustomIndex;

public class ConfigureCustomIndexOptions : IConfigureNamedOptions<LuceneDirectoryIndexOptions>
{
    private readonly IOptions<IndexCreatorSettings> _settings;
    public ConfigureCustomIndexOptions(IOptions<IndexCreatorSettings> settings)
    {
        _settings = settings;
    }
    public void Configure(string name, LuceneDirectoryIndexOptions options)
    {
        if (!name.Equals("TodoIndex"))
            return;

        options.Analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
        options.FieldDefinitions = new FieldDefinitionCollection(
            new FieldDefinition("userID", FieldDefinitionTypes.Integer),
            new FieldDefinition("id", FieldDefinitionTypes.Integer),
            new FieldDefinition("title", FieldDefinitionTypes.FullTextSortable),
            new FieldDefinition("completed", FieldDefinitionTypes.FullTextSortable));
        options.UnlockIndex = true;
        if (_settings.Value.LuceneDirectoryFactory == LuceneDirectoryFactory.SyncedTempFileSystemDirectoryFactory)
        {
            // if this directory factory is enabled then a snapshot deletion policy is required
            options.IndexDeletionPolicy = new SnapshotDeletionPolicy(new KeepOnlyLastCommitDeletionPolicy());

        }
    }
    public void Configure(LuceneDirectoryIndexOptions options)
    {
        throw new NotImplementedException("This is never called and is just part of the interface");
    }

}