// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pokefans.Data;

namespace Pokefans.Areas.mitarbeit.Models
{
    public class UserAdvertisingViewModel
    {
        /// <summary>
        /// Gets or sets the recorded advertisings.
        /// </summary>
        /// <value>
        /// The recorded advertisings.
        /// </value>
        public List<UserAdvertising> RecordedAdvertisings { get; set; }

        /// <summary>
        /// Gets or sets the advertising to add.
        /// </summary>
        /// <value>
        /// The advertising to add.
        /// </value>
        public UserAddAdvertisingViewModel AdvertisingToAdd { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public User User { get; set; }
    }
}