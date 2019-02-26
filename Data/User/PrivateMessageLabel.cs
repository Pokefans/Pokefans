using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.UserData
{
    public class PrivateMessageLabel
    {
		[Column("id")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int Id { get; set; }

        public int OwnerUserId { get; set; }

        [ForeignKey("OwnerUserId")]
        public User Owner { get; set; }

        [MaxLength(50)]
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the color. RGBA, that is, #AARRGGBB 9 chars.
        /// </summary>
        /// <value>The color.</value>
        [MaxLength(9)]
        public string Color { get; set; }
    }
}
