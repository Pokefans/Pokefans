using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data.Forum
{
    public class ThreadPrefix
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Prefix { get; set; }

        [MaxLength(20)]
        public string CSS { get; set; }
    }
}
