using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data.Forum
{
    public class BoardEnabledPrefixes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int BoardId { get; set; }

        public int PrefixId { get; set; }

        public bool IsUserVisible { get; set; }
    }
}
