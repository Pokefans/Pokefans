// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Pokefans.Data
{
    [Table("system_user_login_providers")]
    public class UserLoginProvider
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("user_id")]
        public int UserId { get; set; }
        
        [Index]
        [Column("provider_name")]
        [MaxLength(100)]
        public string ProviderName { get; set; }

        [Index]
        [Column("provider_key")]
        [MaxLength(200)]
        public string ProviderKey { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
