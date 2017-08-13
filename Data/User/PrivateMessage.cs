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
    }
}