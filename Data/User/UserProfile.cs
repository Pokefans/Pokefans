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

        public string JabberId { get; set; }

        public string IrcOnlineTime { get; set; }

        public string GooglePlusUrl { get; set; }

        public int Age { get; set; }

        public string Hometown { get; set; }

        public string Gender { get; set; }

        public string Interests { get; set; }

        public string FoundedIrcChannels { get; set; }

        public string FavoritePokemon { get; set; }

        public string Homestate { get; set; }

        public string IcqNumber { get; set; }

        public string PokemonExperteName { get; set; }

        public string FirstName { get; set; }

        public string Occupation { get; set; }

        public string PokemonShowdownName { get; set; }

        public string BisafansName { get; set; }

        public string FilbBoardName { get; set; }

        public string IrcNickname { get; set; }

        public string IrcAlternativeNickname { get; set; }

        public string IrcChannels { get; set; }

        public string SmogonName { get; set; }

        public string IrcHostmask { get; set; }

        public string SerebiiName { get; set; }

        public string OldUsername { get; set; }

        public string TwitterUrl { get; set; }

        public string FacebookUrl { get; set; }

        public string DeviantArtUrl { get; set; }

        public string YoutubeUrl { get; set; }

        public string SkypeUrl { get; set; }

        public string SoupIoUrl { get; set; }

        public string LastFmName { get; set; }

        public string PinterestUrl { get; set; }

        public string FriendfeedName  { get; set; }

        public string DropIoName { get; set; }

        public string WerKenntWenUrl { get; set; }
    }
}