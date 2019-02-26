using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.UserData
{
    /// <summary>
    ///  This Table solely exists that the sender can add his own lables.
    /// </summary>
    public class PrivateMessageSent
    {
		[Column("id")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int Id { get; set; }

		public int PrivateMessageId { get; set; }

		[ForeignKey("PrivateMessageId")]
		public PrivateMessage Message { get; set; }

		public int UserFromId { get; set; }

		[ForeignKey("UserFromId")]
		public User From { get; set; }

        [InverseProperty("Message")]
        public List<PrivateMessageSentLabel> Labels { get; set; }
    }
}
