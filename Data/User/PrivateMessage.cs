using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.UserData
{
    public class PrivateMessage
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Subject { get; set; }

        public string Body { get; set; }

        public string BodyRaw { get; set; }

        public DateTime Sent { get; set; }

        public Guid ConversationId { get; set; }

        public int ReplyTo { get; set; }

        public string ToLine { get; set; }

        [MaxLength(39)]
        public string SenderIpAddress { get; set; }
    }
}