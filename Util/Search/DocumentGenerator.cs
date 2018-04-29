// Copyright 2016 the pokefans authors. See copying.md for legal info.
using Lucene.Net.Documents;
using Pokefans.Data;
using Pokefans.Data.Fanwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokefans.Data.Wifi;
using Pokefans.Data.Forum;

namespace Pokefans.Util.Search
{
    public class DocumentGenerator
    {
        public static Document WifiOffer(NormalOffer o) 
        {
            Document doc = new Document();

            doc.Add(new Field("type", "fanert", Field.Store.NO, Field.Index.NOT_ANALYZED_NO_NORMS));
            doc.Add(new Field("Id", o.Id.ToString(), Field.Store.YES, Field.Index.NO));

            // let's store just enough so we can display results without a database roundtrip
            doc.Add(new Field("title", o.Title, Field.Store.YES, Field.Index.ANALYZED));

            if(o.Pokemon != null)
            {
                doc.Add(new Field("pokemon", o.Pokemon.Name.German, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("pokemon", o.Pokemon.Name.English, Field.Store.NO, Field.Index.ANALYZED));
                doc.Add(new Field("pokemonid", o.PokemonId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));
            }

            if(o.Item != null)
            {
                doc.Add(new Field("item", o.Item.Name.German, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("item", o.Item.Name.English, Field.Store.NO, Field.Index.ANALYZED));
            }

            doc.Add(new Field("user", o.User.UserName, Field.Store.YES, Field.Index.ANALYZED_NO_NORMS));
            doc.Add(new Field("userid", o.UserId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("originaltrainer", o.IsOriginalTrainer.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));

            if(o.Pokeball != null)
            {
                doc.Add(new Field("pokeball", o.Pokeball.Name, Field.Store.NO, Field.Index.ANALYZED));
            }

            doc.Add(new Field("shiny", o.IsShiny.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));
            doc.Add(new Field("event", o.IsEvent.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));
            doc.Add(new Field("pokerus", o.HasPokerus.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));
            doc.Add(new Field("cheat", o.CheatUsed.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));
            doc.Add(new Field("clone", o.IsClone.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));
            doc.Add(new Field("bottlecaps", o.BottleCapsUsed.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));

            if(o.Ability != null) 
            {
                doc.Add(new Field("fahigkeit", o.Ability.Name.German, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("fahigkeit", o.Ability.Name.English, Field.Store.NO, Field.Index.ANALYZED));
            }

            if(o.Attack1 != null) 
            {
                doc.Add(new Field("attacke", o.Attack1.Name.German, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("attacke", o.Attack1.Name.English, Field.Store.NO, Field.Index.ANALYZED));
            }

            if(o.Attack2 != null)
            {
                doc.Add(new Field("attacke", o.Attack2.Name.German, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("attacke", o.Attack2.Name.English, Field.Store.NO, Field.Index.ANALYZED));
            }

            if(o.Attack3 != null)
            {
                doc.Add(new Field("attacke", o.Attack3.Name.German, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("attacke", o.Attack3.Name.English, Field.Store.NO, Field.Index.ANALYZED));
            }

            if(o.Attack4 != null)
            {
                doc.Add(new Field("attacke", o.Attack4.Name.German, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("attacke", o.Attack4.Name.English, Field.Store.NO, Field.Index.ANALYZED));
            }

            doc.Add(new Field("evs", o.EffortValues.Any().ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));
            doc.Add(new Field("dvs", o.DeterValues.Any().ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));

            doc.Add(new Field("generation", ((int)o.Generation).ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));
            doc.Add(new Field("time", o.UpdateTime.ToString("o"), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));

            return doc;
        }


        public static Document Fanart(Fanart fanart)
        {
            Document doc = new Document();

            // classify this document as fanart. we only need it indexed for it to be searchable as a condition,
            // the value itself is pretty pointless to store.
            doc.Add(new Field("type", "fanart", Field.Store.NO, Field.Index.NOT_ANALYZED_NO_NORMS));

            doc.Add(new Field("Id", fanart.Id.ToString(), Field.Store.YES, Field.Index.NO));

            // We store some fields so we don't need a database roundtrip to display search results.
            doc.Add(new Field("title", fanart.Title, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("user", fanart.UploadUser.UserName, Field.Store.YES, Field.Index.ANALYZED_NO_NORMS));
            // color can be retrieved but ís not in the index (i.e. not searchable)
            doc.Add(new Field("usercolor", fanart.UploadUser.Color ?? "", Field.Store.YES, Field.Index.NO));
            doc.Add(new Field("userid", fanart.UploadUserId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            // store the thumbnail urls, but again: searching them is pointless.
            doc.Add(new Field("smallThumbnailUrl", fanart.SmallThumbnailUrl, Field.Store.YES, Field.Index.NO));
            // large thumbnail url is saved for "related work" sidebar.
            doc.Add(new Field("largeThumbnailUrl", fanart.LargeThumbnailUrl, Field.Store.YES, Field.Index.NO));
            // the rating is stored and indexable.
            NumericField rf = new NumericField("rating", Field.Store.YES, true);
            rf.SetFloatValue((float)fanart.Rating);
            doc.Add(rf);
            // saving the category id we need for searching anyways is enough to recover the uri from the cache.
            doc.Add(new Field("categoryId", fanart.CategoryId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

            // searchable, but not relevant in the search results.
            doc.Add(new Field("description", fanart.DescriptionCode, Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field("category", fanart.Category.Name, Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field("uploaddate", DateTools.DateToString(fanart.UploadTime, DateTools.Resolution.DAY), Field.Store.NO, Field.Index.ANALYZED));

            // Index all tags for this fanart. 
            string[] tags = fanart.Tags.Select(x => x.Tag.Name).ToArray();
            foreach (string tag in tags)
            {
                doc.Add(new Field("tag", tag, Field.Store.NO, Field.Index.ANALYZED));
            }
            return doc;
        }

        public static Document Content(Content content)
        {
            Document doc = new Document();
            doc.Add(new Field("contentType", content.Type.ToString(), Field.Store.NO, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("type", "content", Field.Store.NO, Field.Index.NOT_ANALYZED_NO_NORMS));

            doc.Add(new Field("Id", content.Id.ToString(), Field.Store.YES, Field.Index.NO));

            // let's store just enough so we can display search results without querying the database
            doc.Add(new Field("title", content.Title, Field.Store.YES, Field.Index.ANALYZED));
            // teaser is split because a field can hold a maximum of 255 characters. teasers may be longer.
            // we only want the first 255 characters in the search results anyway. but the whole thing needs to be
            // searchable
            doc.Add(new Field("teaserDisplay", content.Teaser.Substring(0, 255), Field.Store.YES, Field.Index.NO));
            doc.Add(new Field("teaser", content.Teaser, Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field("primaryURI", content.Urls.First(x => x.Type == UrlType.Default).Url, Field.Store.YES, Field.Index.NO));

            // needs to be indexed, but not retrieved
            doc.Add(new Field("content", content.ParsedContent, Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field("category", content.CategoryId.ToString(), Field.Store.NO, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("author", content.Author.UserName, Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field("authorId", content.AuthorUserId.ToString(), Field.Store.NO, Field.Index.NOT_ANALYZED));
            NumericField status = new NumericField("status", Field.Store.NO, true);
            status.SetIntValue((int)content.Status);
            doc.Add(status);

            return doc;
        }

        public static Document Post(Post post) {
            Document doc = new Document();

            doc.Add(new Field("type", "forumPost", Field.Store.NO, Field.Index.NOT_ANALYZED_NO_NORMS));
            doc.Add(new Field("Id", post.Id.ToString(), Field.Store.YES, Field.Index.NO));
            doc.Add(new Field("title", post.Subject, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("forumId", post.Thread.BoardId.ToString(), Field.Store.YES, Field.Index.NO));
            doc.Add(new Field("content", post.Body, Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field("author", post.Author.UserName, Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field("authorId", post.AuthorId.ToString(), Field.Store.YES, Field.Index.NO));
            doc.Add(new Field("isSolution", post.IsSolution.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

            return doc;
        }
    }
}
