using System;
namespace Pokefans.Areas.user.Models.Feed
{
    public class FanartCommentFeedContent : FanartFeedContent, ICommentFeedContent
    {
        public string Comment { get; set; }
    }
}
