// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pokefans.Data;

namespace Pokefans.Areas.mitarbeit.Models
{
    public class UserAddNoteViewModel
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the note to add.
        /// </summary>
        /// <value>
        /// The note to add.
        /// </value>
        public UserNote NoteToAdd { get; set; }

        /// <summary>
        /// Gets or sets the note actions.
        /// </summary>
        /// <value>
        /// The note actions.
        /// </value>
        public List<SelectListItem> NoteActions { get; set; }

        /// <summary>
        /// Gets or sets the BVS roles.
        /// </summary>
        /// <value>
        /// The BVS roles.
        /// </value>
        public List<SelectListItem> BvsRoles { get; set; }
    }
}