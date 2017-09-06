using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data.User
{
    public class UserFollower
    {
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		[Column("id")]
		public int Id { get; set; }

        /// <summary>
        /// Identifier for the user that follows another user.
        /// </summary>
        /// <value>The follower identifier.</value>
        public int FollowerId { get; set; }

        [ForeignKey("UserId")]
        public User Follower { get; set; }

        /// <summary>
        /// Identifier for the user that is being followed.
        /// </summary>
        /// <value>The followed identifier.</value>
        public int FollowedId { get; set; }

        [ForeignKey("FollowedId")]
        public User Followed { get; set; }
    }
}
