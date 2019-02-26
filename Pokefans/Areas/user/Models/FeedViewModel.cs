using System;
using System.Collections.Generic;
using Pokefans.Areas.user.Models.Feed;
using System.Linq;
namespace Pokefans.Areas.user.Models
{
    public class FeedViewModel
    {
        public FeedViewModel(List<IFeedContent> content) {
            FeedContent = content;
        }

        private List<IFeedContent> _feedContent;
        public List<IFeedContent> FeedContent { 
            get {
                return _feedContent;        
            } 
            set {
                _feedContent = value;

                CalendarOffset = _feedContent.Count(x => x.GetType() == typeof(CalendarFeedContent));
                FanartCommentOffset = _feedContent.Count(x => x.GetType() == typeof(FanartCommentFeedContent));
                FanartOffset = _feedContent.Count(x => x.GetType() == typeof(FanartFeedContent));
                WifiOfferOffset = _feedContent.Count(x => x.GetType() == typeof(NewWifiOfferFeedContent));
                PokedexCommentOffset = _feedContent.Count(x => x.GetType() == typeof(PokedexCommentFeedContent));
                NewsCommentOffset = _feedContent.Count(x => x.GetType() == typeof(NewsCommentFeedContent));
            } 
        }

        public int CalendarOffset { get; private set; }
        public int FanartCommentOffset { get; private set; }
        public int FanartOffset { get; private set; }
        public int WifiOfferOffset { get; private set; }
        public int PokedexCommentOffset { get; private set; }
        public int NewsCommentOffset { get; private set; }
    }
}
