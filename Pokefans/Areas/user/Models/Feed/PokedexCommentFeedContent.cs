// Copyright 2017 the pokefans authors. See copying.md for legal info.
using System;

namespace Pokefans.Areas.user.Models.Feed
{
    public class PokedexCommentFeedContent : IFeedContent, ICommentFeedContent
    {
        public DateTime Timestamp { get; set; }
        public string Username { get; set; }
        public string Url { get; set; }
        public string AvatarUrl { get; set; }

        public string Comment { get; set; }
        public string PokemonUrl { get; set; }
    }
}
