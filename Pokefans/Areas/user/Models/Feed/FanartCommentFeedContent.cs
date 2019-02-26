// Copyright 2017 the pokefans authors. See copying.md for legal info.
using System;
namespace Pokefans.Areas.user.Models.Feed
{
    public class FanartCommentFeedContent : FanartFeedContent, ICommentFeedContent
    {
        public string Comment { get; set; }
    }
}
