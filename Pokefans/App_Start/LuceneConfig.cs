// Copyright 2016 the pokefans authors. See copying.md for legal info
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.De;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Pokefans
{
    /// <summary>
    /// This class configures all search indexes used in pokefans and registers them in the
    /// Unity container for easy dependency injection.
    /// </summary>
    public class LuceneConfig
    {
        public static void Configure(IUnityContainer container)
        {
            Directory luceneDir = FSDirectory.Open(ConfigurationManager.AppSettings["SearchIndexPath"]);
            GermanAnalyzer Analyzer = new GermanAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            IndexWriter writer = new IndexWriter(luceneDir, Analyzer, new IndexWriter.MaxFieldLength(255));
            IndexSearcher searcher = new IndexSearcher(luceneDir);

            container.RegisterInstance(writer);
            container.RegisterInstance<Searcher>(searcher);
            container.RegisterInstance<Analyzer>(Analyzer);
            container.RegisterInstance(luceneDir);
            
        }

        /// <summary>
        /// Cleanup the indexes.
        /// </summary>
        /// <param name="unityContainer"></param>
        internal static void Unload(IUnityContainer unityContainer)
        {
            IndexWriter writer = unityContainer.Resolve<IndexWriter>();
            writer.Dispose();

            Searcher searcher = unityContainer.Resolve<Searcher>();
            searcher.Dispose();

            Directory dir = unityContainer.Resolve<Directory>();
            dir.Dispose();
        }
    }
}