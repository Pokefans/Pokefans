// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pokefans.Areas.mitarbeit.Models
{
    public class UserAddAdvertisingViewModel
    {
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The advertised URL.
        /// </value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the advertising form identifier.
        /// </summary>
        /// <value>
        /// The advertising form identifier.
        /// </value>
        public int AdvertisingFormId { get; set; }

        /// <summary>
        /// Gets or sets the name of the target user. Only applicable if the advertising form is targeted.
        /// </summary>
        /// <value>
        /// The name of the target user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the screenshot URL.
        /// </summary>
        /// <value>
        /// The screenshot URL (if any).
        /// </value>
        public string ScreenshotUrl { get; set; }
    }
}