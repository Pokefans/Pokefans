// Copyright 2017 the pokefans authors. See copyright.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Pokefans.Data.UserData;
using Pokefans.Data;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Pokefans.Areas.user.Models.Feed;
using Pokefans.Areas.user.Models;
using Pokefans.Data.Comments;
using Pokefans.Util.Comments;
using Pokefans.Util;


namespace Pokefans.Areas.user.Controllers
{
	[Authorize]
    public class ProfileController : Controller
    {
		Entities db;

        public ProfileController(Entities ents) {
            db = ents;
        }

        public ActionResult Index() {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());

            // Maybe these queries need optimization, but let's time it first.
            UserFeedConfig config = db.UserFeedConfigs.Where(x => x.UserId == uid).First();
            int[] followedUserIds = db.UserFollowers.Where(x => x.FollowerId == uid).Select(x => x.FollowedId).ToArray();

            List<IFeedContent> feedContent = new List<IFeedContent>();

            // That comparison works because the fields are bitmasks,
            // if nothing is to be followed all bits are not set i.e it's zero.
            if (config.NewFanart > UserFeedConfig.Visibility.Nobody) {
                // TODO: maybe find better way than join. But first measure.
                var fanartQuery = db.Fanarts.Include("UploadUser").AsEnumerable();

                // If anything goes wrong or we have unidentified Followingtype,
                // then fallback to display only own fanarts.
                if ((config.NewFanart ^ UserFeedConfig.Visibility.All) > 0)
                {
                    if ((config.NewFanart & UserFeedConfig.Visibility.Following) > 0)
                    {
                        if ((config.NewFanart & UserFeedConfig.Visibility.Own) > 0)
                        {
                            fanartQuery = fanartQuery.Where(x => followedUserIds.Contains(x.UploadUserId) || x.UploadUserId == uid);
                        }
                        else
                        {
                            fanartQuery = fanartQuery.Where(x => followedUserIds.Contains(x.UploadUserId));
                        }
                    }
                    else
                    {
                        // Own and Default.
                        fanartQuery = fanartQuery.Where(x => x.UploadUserId == uid);
                    }
                }


                var result = fanartQuery.Take(15).OrderByDescending(x => x.UploadTime).ToList();

                foreach (var fanart in result) {
                    feedContent.Add(new FanartFeedContent()
                    {
                        Fanart = fanart,
                        Username = fanart.UploadUser.UserName,
                        AvatarUrl = fanart.UploadUser.AvatarUrl,
                        Url = fanart.UploadUser.Url,
                        Timestamp = fanart.UploadTime
                    });
                }
            }

            // In the linear feed, show only top level comments. Replies to
            // other existing comments are handled in the notification feed
            // and should always redirect to the specific fanart page,
            // because most likely the threads need some kind of context
            // which would introduce too much clutter in the linear feed.
            if (config.CommentsOnFanart > UserFeedConfig.Visibility.Nobody)
            {
                // The Visibility constraint here also applies to the base
                // fanart, *not* the comment itself, i.e. you see comments to your
                // own fanart, friends' fanarts or all fanarts (which may very
                // well won't be a good idea)

                // Because of the way the comment system is built, filtering
                // the comments is a bit involved; at least from the ORM side of
                // things as the comment system in general does not map too well.
                // This will now be a huge join but eh.
                // Not ideal, but works for the moment. This query can and should
                // be optimized of course.

                List<int> authors = new List<int>();
                if ((config.CommentsOnFanart & UserFeedConfig.Visibility.Following) > 0)
                    authors.AddRange(followedUserIds);
                if ((config.CommentsOnFanart & UserFeedConfig.Visibility.Own) > 0)
                    authors.Add(uid);

                bool showAll = (config.CommentsOnFanart & UserFeedConfig.Visibility.All) > 0;

                var comments = (from a in db.Comments
                                join f in db.Fanarts.Include("UploadUserId") on a.CommentedObjectId equals f.Id
                                join u in db.Users on a.AuthorId equals u.Id
                                where a.Context == (int)CommentContext.Fanart
                                 && (showAll || authors.Contains(f.UploadUserId))
                                 // && a.Children.Count() == 0
                                orderby a.SubmitTime descending
                                select new { Comment = a, Fanart = f, Author = u }).Take(15);

                foreach(var comment in comments) {
                    feedContent.Add(new FanartCommentFeedContent()
                    {
                        Fanart = comment.Fanart,
                        Username = comment.Author.UserName,
                        AvatarUrl = comment.Author.AvatarUrl,
                        Url = comment.Author.Url,
                        Timestamp = comment.Comment.SubmitTime,
                        Comment = comment.Comment.ParsedComment
                    });
                }
            }

            if (config.NewWifiOffers > UserFeedConfig.Visibility.Nobody) 
            {
                // Visibility constraint applies to the offer author
                List<int> authors = new List<int>();
                if((config.NewWifiOffers & UserFeedConfig.Visibility.Own) > 0) {
                    authors.Add(uid);
                }
                if((config.NewWifiOffers & UserFeedConfig.Visibility.Following) > 0) {
                    authors.AddRange(followedUserIds);
                }

                bool showAll = (config.NewWifiOffers & UserFeedConfig.Visibility.All) > 0;

                var trades = (from a in db.WifiOffers.Include("User")
                              where (showAll || authors.Contains(a.UserId)) && a.Status == Data.Wifi.TradingStatus.Offer
                              orderby a.UpdateTime descending
                              select a).Take(15);

                foreach(var trade in trades) {
                    feedContent.Add(new NewWifiOfferFeedContent()
                    {
                        Url = trade.User.Url,
                        Username = trade.User.UserName,
                        AvatarUrl = trade.User.AvatarUrl,
                        Timestamp = trade.UpdateTime,
                        Offer = trade
                    });
                }
            }

            if (config.CommentsOnNews > UserFeedConfig.Visibility.Nobody)
            {
                // Based on activity - either written by or commenting on counts as activity.
                List<int> authors = new List<int>();
                if ((config.CommentsOnNews & UserFeedConfig.Visibility.Own) > 0)
                {
                    authors.Add(uid);
                }
                if ((config.CommentsOnNews & UserFeedConfig.Visibility.Following) > 0)
                {
                    authors.AddRange(followedUserIds);
                }

                bool showAll = (config.CommentsOnNews & UserFeedConfig.Visibility.All) > 0;

                string authorlist = authors.Aggregate("", (acc, i) => acc.Length == 0 ? acc + "'" + i.ToString() + "'" : acc + ",'" + i.ToString() + "'");
                if (authorlist.Trim().Length == 0)
                    authorlist = "-2";

                // Note: I know this is bad practice, but strong type safety protects us here.
                // Unfortunately, EF does not support the "IN" keyword.
                var news = db.Comments.SqlQuery(
                    @"SELECT * from Comments c
                      LEFT JOIN system_users u on c.AuthorId = u.id
                      LEFT JOIN content i on c.CommentedObjectId = i.Id
                      WHERE (@p0 OR c.AuthorId IN (" + authorlist + @")
                         OR i.AuthorUserId IN(" + authorlist + @"))
                        AND i.Type = @p1
                      ORDER BY c.SubmitTime DESC
                      LIMIT 20", new { showAll, ContentType.News});
                
                foreach(var n in news) {
                    feedContent.Add(new NewsCommentFeedContent() {
                        Username = n.Author.UserName,
                        AvatarUrl = n.Author.AvatarUrl,
                        Comment = n.ParsedComment,
                        Url = n.Author.Url,
                        ContentUrl = Url.Map("/inhalt/"+n.CommentedObjectId.ToString(), null)
                    });
                }
            }

            // TODO - Calendar: omitted for now because abonnement level should depend on Categories.
            // TODO - Forum:    ommited for now because abbonement level should depend on subforum.
            // and more, whatever.

            // paging is solved partly by the viewmodel, which supplies counts of the various items we've selected.
            // upon requesting more items, these offsets will be taken into consideration.
            return View("~/Areas/user/Views/Profile/Feed.cshtml", new FeedViewModel(feedContent.OrderByDescending(g => g.Timestamp).Take(20).ToList()));
        }

        public ActionResult ViewProfile(string url) {
            // We need to get a UID first.
            int uid = -1;

            if(!int.TryParse(url, out uid)) {
                var nuid = db.Users.Where(x => x.Url == url).FirstOrDefault();
                if(nuid == null)
                    return View("~/Areas/user/Views/Profile/ViewProfile.cshtml", null);
                uid = nuid.Id;
            }
			// uid is now guaranteed to be different to -1: either it was numeric in the first place,
			// then it's set and it's fine and dandy.
			// or we have - at this point - found a user which corresponds to this name.
            // if the id is invalid, the following query will return null, which
            // in turn will trigger the unknown user message to be displayed.

			var profile = db.UserProfile.Include("User").Where(x => x.UserId == uid).FirstOrDefault();

			return View("~/Areas/user/Views/Profile/ViewProfile.cshtml", profile);
        }
    }
}
