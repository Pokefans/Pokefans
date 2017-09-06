// Copyright 2017 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Pokefans.Data.UserData
{
    public class UserFeedConfig
    {
		/// <summary>
		/// Storing enums in the database is a tedious process and is simplified
        /// if we just make an inner class containing fields with the desired
        /// values. These values hold true for every field config out there,
        /// as outlined in this class or the appropriate feed classes of the
        /// subsystems. There are certain fields where there is just "all" or
        /// "none", for example everything in the calendar. However, this is
        /// also done in the calendar subsystem, as there are multiple categories
        /// where one should be able to define wether he/she is interested in
        /// including them in the feed.
		/// </summary>
		public class Visibility
        {
            public const short Nobody = 0;
            public const short Own = 1;
            public const short Friends = 2;
            public const short All = 4;
        }

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        /// <summary>
        /// Indicates wether the user has customized the feed once.
        /// </summary>
        /// <value><c>true</c> if changed settings; otherwise, <c>false</c>.</value>
        public bool ChangedSettings { get; set; }

        public short NewFanart { get; set; }

        public short CommentsOnFanart { get; set; }

    }
}
