// Copyright 2017 the pokefans authors. See copying.md for legal info.
using System;
using Pokefans.Data.Calendar;

namespace Pokefans.Areas.user.Models.Feed
{
    public class CalendarFeedContent : IFeedContent
    {

        public DateTime Timestamp { get; set; }
        public string Username { get; set; }

        public Appointment Appointment { get; set; }
    }
}
