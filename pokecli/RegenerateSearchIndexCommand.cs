// Copyright 2016 the pokefans authors. See copying.md for legal info.
using Lucene.Net.Analysis.De;
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Store;
using ManyConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Pokefans.Data;
using Pokefans.Data.Fanwork;
using Lucene.Net.Documents;
using Pokefans.Util.Search;

namespace pokecli
{
    class RegenerateSearchIndexCommand : ConsoleCommand
    {
        public RegenerateSearchIndexCommand()
        {
            IsCommand("regen-search-index", "Regenerates the lucene.net search index from the database.");
        }

        public override int Run(string[] remainingArguments)
        {
            const int n = 2;
            Directory luceneDir = FSDirectory.Open(ConfigurationManager.AppSettings["SearchIndexPath"]);
            Analyzer Analyzer = new GermanAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            IndexWriter writer = new IndexWriter(luceneDir, Analyzer, true, new IndexWriter.MaxFieldLength(255));
            Entities db = new Entities();


            Console.WriteLine($"[1/{n}] Writing Fanart Index...");

            // for performance reasons, we process only 1000 items at a time
            int max = db.Fanarts.Count();

            for (int i = 0; i < max; i = i + 1000)
            {
                // yes, this will be a heavvily unoptimized query which loads way too much.
                // Also, I don't care. It does not matter if it takes a second or even ten seconds,
                // we don't need to serve clients here.
                List<Fanart> fanarts = db.Fanarts
                                           .Include("Tags")
                                           .Include("Tags.Tag")
                                           .Include("UploadUser")
                                           .Include("Category")
                                           .OrderBy(x => x.Id).Skip(i).Take(1000).ToList();

                foreach (var fanart in fanarts)
                {
                    writer.AddDocument(DocumentGenerator.Fanart(fanart));
                }
                writer.Flush(true, true, true);
            }

            // write content index
            Console.WriteLine($"[2/{n}] Writing Content Index...");
            max = db.Contents.Where(x => x.Type == ContentType.News).Count();

            for (int i = 0; i < max; i = i + 1000)
            {
                var Contents = db.Contents.Include("Author").Where(x => x.Type == ContentType.News).Skip(i).Take(1000);

                foreach (var content in Contents)
                {
                    Document doc = DocumentGenerator.Content(content);
                    writer.AddDocument(doc);
                }

                writer.Flush(true, true, true);
            }

            writer.Dispose();
            luceneDir.Dispose();

            return 0;
        }

    }
}
