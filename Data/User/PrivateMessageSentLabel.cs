﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.UserData
{
    public class PrivateMessageSentLabel
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int PrivateMessageLabelId { get; set; }

        [ForeignKey("PrivateMessageLabelId")]
        public PrivateMessageLabel Label { get; set; }

        public int PrivateMessageSentId { get; set; }

        [ForeignKey("PrivateMessageSentId")]
        public PrivateMessageSent Message { get; set; }
    }
}
