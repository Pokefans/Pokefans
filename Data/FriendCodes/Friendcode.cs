// Copyright 2015 the pokefans authors. See copying.md for legal info.using System;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.FriendCodes
{
    public class Friendcode
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public int GameId { get; set; }

        [ForeignKey("GameId")]
        public FriendcodeGame Game { get; set; }

        [MaxLength(16)]
        public string FriendCode { get; set; }

        public string IngameName { get; set; }

        public bool IsPublic { get; set; }

        public DateTime LastUpate { get; set; }
    }
}

