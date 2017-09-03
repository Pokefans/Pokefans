// Copyright 2017 the pokefans authors. See copying.md for legal info.
using System;
namespace Pokefans.Areas.user.Models.Feed
{
    public interface IFeedContent
    {
        DateTime Timestamp { get; set; }

        string Username { get; set; }
    }
}
