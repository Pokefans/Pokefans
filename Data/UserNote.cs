// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Data
{
    [Table("user_notes")]
    public class UserNote
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("content")]
        public string Content { get; set; }

        [Column("unparsed_content")]
        public string UnparsedContent { get; set; }

        [Required]
        [Column("action_id")]
        public int ActionId { get; set; }

        [Column("author_id")]
        public int AuthorId { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("creation_time")]
        public DateTime Created { get; set; }

        [Required]
        [Column("role_id_needed")]
        public int RoleIdNeeded { get; set; }

        [Required]
        [Column("deleteable")]
        public bool IsDeletable { get; set; }

        [ForeignKey("RoleIdNeeded")]
        public virtual Role RoleNeeded { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }
    }
}
