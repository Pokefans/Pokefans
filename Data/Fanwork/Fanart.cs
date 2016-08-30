// Copyright 2015-2016 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Pokefans.Data.Pokedex;
using System.Data.Entity;
using System.Collections.Generic;

namespace Pokefans.Data.Fanwork
{
    public enum FanartStatus
    {
        OK = 0,
        Deleted = -1
    }

    ;

    public class Fanart
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public FanartStatus Status { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public FanartCategory Category { get; set; }

        [MaxLength(255)]
        public string Url { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }

        public string DescriptionCode { get; set; }

        public FanartDimension Size { get; set; }

        public decimal Rating { get; set; }

        public int RatingCount { get; set; }

        public decimal SmartRating { get; set; }

        public short Order { get; set; }

        public bool IsTileset { get; set; }

        public bool Protect { get; set; }

        public int? PokemonId { get; set; }

        [ForeignKey("PokemonId")]
        public Pokemon Pokemon { get; set; }

        public DateTime UploadTime { get; set; }

        public int UploadUserId { get; set; }

        [MaxLength(47)]
        public string UploadIp { get; set; }

        public int FileSize { get; set; }

        public int? ChallengeId { get; set; }

        public int CommentCount { get; set; }

        private ICollection<FanartTags> tags;

        [InverseProperty("Fanart")]
        public virtual ICollection<FanartTags> Tags
        {
            get { return tags ?? (tags = new HashSet<FanartTags>()); }
            set { tags = value; }
        }

        [ForeignKey("UploadUserId")]
        public User UploadUser { get; set; }

        [ForeignKey("ChallengeId")]
        public FanartChallenge Challenge { get; set; }

        public static void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fanart>().Property(g => g.Size.X).HasColumnName("image_width");
            modelBuilder.Entity<Fanart>().Property(g => g.Size.Y).HasColumnName("image_height");
        }

        private ICollection<FanartRating> ratings;

        [InverseProperty("Fanart")]
        public virtual ICollection<FanartRating> Ratings
        {
            get { return ratings ?? (ratings = new HashSet<FanartRating>()); }
            set { ratings = value; }
        }

        [NotMapped]
        public string SmallThumbnailUrl
        {
            get
            {
                return Url.Split('.')[0] + "_t4.png";
            }
        }

        [NotMapped]
        public string LargeThumbnailUrl
        {
            get
            {
                return Url.Split('.')[0] + "_t2.png";
            }
        }
    }
}