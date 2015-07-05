// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pokefans.Data;

namespace Pokefans.Areas.mitarbeit.Models
{
    public class UserMultiaccountViewModel
    {
        /// <summary>
        /// Gets or sets the multiaccount to add.
        /// </summary>
        /// <value>
        /// The multiaccount to add.
        /// </value>
        public UserMultiaccountAddViewModel MultiaccountToAdd { get; set; }

        /// <summary>
        /// Gets or sets the accounts.
        /// </summary>
        /// <value>
        /// The accounts.
        /// </value>
        public List<UserMultiaccount> Accounts { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public User User { get; set; }
    }
}