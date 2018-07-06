using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Pokefans.Data.UserData
{
    public class PrivateMessageInbox
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

        public int UserToId { get; set; }

        [ForeignKey("UserToId")]
        public User To { get; set; }

        public bool Read { get; set; }

        public bool IsBcc { get; set; }

        [InverseProperty("Message")]
        public List<PrivateMessageInboxLabel> Labels { get; set; }
    }
}
