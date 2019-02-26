using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Pokefans.Data.UserData
{
    public class PrivateMessageReport
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int PrivateMessageId { get; set; }

        public int UserFromId { get; set; }

        public int UserReportId { get; set; }

        [ForeignKey("UserFromId")]
        public User From { get; set; }

        [ForeignKey("UserReportId")]
        public User Reporter { get; set; }

        [ForeignKey("PrivateMessageId")]
        public PrivateMessage PrivateMessage { get; set; }

        public string Details { get; set; }

        public DateTime Timestamp { get; set; }

        public bool Resolved { get; set; }

        public int? UserResolveId { get; set; }

        [ForeignKey("UserResolveId")]
        public User Resolver { get; set; }

        public DateTime? ResolveTime { get; set; }

        public string ModeratorNotes { get; set; }

        [MaxLength(39)]
        public string IpAddress { get; set; }
    }
}
