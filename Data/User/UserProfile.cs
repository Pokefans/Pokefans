using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.UserData
{
    public class UserProfile
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public bool CkEditorShowSource { get; set; }

        [MaxLength(100)]
        public string JabberId { get; set; }

        [MaxLength(100)]
        public string IrcOnlineTime { get; set; }

        [MaxLength(100)]
        public string GooglePlusUrl { get; set; }

        public int Age { get; set; }

        [MaxLength(100)]
        public string Hometown { get; set; }

        [MaxLength(100)]
        public string Gender { get; set; }

        [MaxLength(255)]
        public string Interests { get; set; }

        [MaxLength(100)]
        public string FoundedIrcChannels { get; set; }

        [MaxLength(100)]
        public string FavoritePokemon { get; set; }

        [MaxLength(100)]
        public string Homestate { get; set; }

        [MaxLength(100)]
        public string IcqNumber { get; set; }

        [MaxLength(100)]
        public string PokemonExperteName { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string Occupation { get; set; }

        [MaxLength(100)]
        public string PokemonShowdownName { get; set; }

        [MaxLength(100)]
        public string BisafansName { get; set; }

        [MaxLength(100)]
        public string FilbBoardName { get; set; }

        [MaxLength(100)]
        public string IrcNickname { get; set; }

        [MaxLength(100)]
        public string IrcAlternativeNickname { get; set; }

        [MaxLength(255)]
        public string IrcChannels { get; set; }

        [MaxLength(100)]
        public string SmogonName { get; set; }

        [MaxLength(100)]
        public string IrcHostmask { get; set; }

        [MaxLength(100)]
        public string SerebiiName { get; set; }

        [MaxLength(100)]
        public string OldUsername { get; set; }

        [MaxLength(100)]
        public string TwitterUrl { get; set; }

        [MaxLength(100)]
        public string FacebookUrl { get; set; }

        [MaxLength(100)]
        public string DeviantArtUrl { get; set; }

        [MaxLength(100)]
        public string YoutubeUrl { get; set; }

        [MaxLength(100)]
        public string SkypeUrl { get; set; }

        [MaxLength(100)]
        public string SoupIoUrl { get; set; }

        [MaxLength(100)]
        public string LastFmName { get; set; }

        [MaxLength(100)]
        public string PinterestUrl { get; set; }

        [MaxLength(100)]
        public string FriendfeedName  { get; set; }

        [MaxLength(100)]
        public string DropIoName { get; set; }

        [MaxLength(100)]
        public string WerKenntWenUrl { get; set; }
    }
}