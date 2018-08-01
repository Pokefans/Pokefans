// Copyright 2016 the pokefans authors. See copying.md for legal info.
using Pokefans.Caching;
using Pokefans.Data;
using Pokefans.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Pokefans.Data.Comments;
using Pokefans.Data.Fanwork;
using Pokefans.Data.ViewModels;
using Pokefans.Data.Wifi;
using System.Web.Mvc;

namespace Pokefans.Util.Comments
{
    public class TradingCommentManager : CommentManager
    {

        NotificationManager notificationManager;
        public TradingCommentManager(Entities ents, Cache c, HttpContextBase b, NotificationManager mgr) : base(ents, c, b) 
        {
            notificationManager = mgr;
        }

        protected override CommentContext context
        {
            get
            {
                return CommentContext.Trading;
            }
        }

        /// <summary>
        /// Adds a new Trading Comment to the database
        /// </summary>
        /// <param name="comment"></param>
        public override void AddComment(Comment comment)
        {
            if(comment.Context != (int)this.context)
            {
                throw new CommentException("This comment must be of type trading");
            }

            Offer o = db.WifiOffers.Single(x => x.Id == comment.CommentedObjectId);
            if(o == null)
            {
                throw new CommentException("Commented Object does not exist");
            }

            // new comments can only be added if the offer is open.
            if (o.Status != TradingStatus.Offer) {
                throw new CommentException("Trade is not open");
            }
            base.AddComment(comment);

            User author = db.Users.Single(x => x.Id == comment.AuthorId);
            UrlHelper helper = new UrlHelper();
            notificationManager.SendNotification(o.UserId,
                                                 string.Format("<a href=\"{0}\">{1}</a> hat <a href=\"{2}\">dein Tauschangebot {3}</a> kommentiert.",
                                                               helper.Map("profil/" + author.Url, "user"),
                                                               author.UserName,
                                                               helper.Map("tausch/" + o.Id.ToString()),
                                                               o.Title
                                                              ),
                                                 "<i class=\"fa fa - refresh\" aria-hidden=\"true\"></i>");

            // TODO: Benachrichtigung senden
        }

        /// <summary>
        /// Additionally, within the fanart scope, Users can delete comments on their own works
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public override bool CanDelete(User currentUser, IComment c)
        {
            if (currentUser == null)
                return false;

            Offer f = db.WifiOffers.Single(x => x.Id == c.CommentedObjectId);

            if (currentUser.Id == f.UserId)
                return true;

            return base.CanDelete(currentUser, c);
        }
    }
}
