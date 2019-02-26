// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pokefans.Data;

namespace Pokefans.Areas.mitarbeit.Models
{
    public class UserRolesViewModel
    {
        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public List<Role> Roles { get; set; }

        /// <summary>
        /// Gets or sets the user roles.
        /// </summary>
        /// <value>
        /// The user roles.
        /// </value>
        public List<UserRole> UserRoles { get; set; }

        /// <summary>
        /// Gets or sets the role chain.
        /// </summary>
        /// <value>
        /// The role chain.
        /// </value>
        public List<RoleChainEntry> RoleChain { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public User User { get; set; }
    }
}