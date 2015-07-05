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
    [Table("user_advertising_forms")]
    public class UserAdvertisingForm
    {
        [Column("id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Column("is_targeted")]
        public bool IsTargeted { get; set; }

        private ICollection<UserAdvertising> userAdvertisings;
        [InverseProperty("AdvertisingForm")]
        public virtual ICollection<UserAdvertising> UserAdvertisings
        {
            get { return userAdvertisings ?? (userAdvertisings = new HashSet<UserAdvertising>()); }
            set { userAdvertisings = value; }
        }
    }
}
