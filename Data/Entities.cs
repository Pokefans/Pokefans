// Copyright 2015-2017 the pokefans authors. See copying.md for legal info.
using Pokefans.Data.Calendar;
using Pokefans.Data.Contents;
using Pokefans.Data.Fanwork;
using Pokefans.Data.FriendCodes;
using Pokefans.Data.Pokedex;
using Pokefans.Data.Pokedex.Ranger;
using Pokefans.Data.Service;
using Pokefans.Data.Strategy;
using Pokefans.Data.Tracker;
using Pokefans.Data.UserData;
using Pokefans.Data.Wifi;


namespace Pokefans.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Diagnostics;
    using System.Linq;
    using MySql.Data.Entity;
    using Comments;
    using Pokefans.Data.Forum;

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class Entities : DbContext
    {
        // Der Kontext wurde für die Verwendung einer Pokefans-Verbindungszeichenfolge aus der
        // Konfigurationsdatei ('App.config' oder 'Web.config') der Anwendung konfiguriert. Diese Verbindungszeichenfolge hat standardmäßig die
        // Datenbank 'Pokefans.Data.Pokefans' auf der LocalDb-Instanz als Ziel.
        //
        // Wenn Sie eine andere Datenbank und/oder einen anderen Anbieter als Ziel verwenden möchten, ändern Sie die Pokefans-Zeichenfolge
        // in der Anwendungskonfigurationsdatei.
        public Entities()
            : base("Entities")
        {
        }

        /// <summary>
        /// This sets the specified Entity modified. It just exists to work around a limitation in Entity Framework (Unit Tests hooraay)
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void SetModified(object entity)
        {
            this.Entry(entity).State = EntityState.Modified;
        }

        // Fügen Sie ein 'DbSet' für jeden Entitätstyp hinzu, den Sie in das Modell einschließen möchten. Weitere Informationen
        // zum Konfigurieren und Verwenden eines Code First-Modells finden Sie unter 'http://go.microsoft.com/fwlink/?LinkId=390109'.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        #region System

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<RoleLogEntry> RoleLogEntries { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<UserLogin> UserLogins { get; set; }

        public virtual DbSet<UserRole> UserRoles { get; set; }

        public virtual DbSet<UserLoginProvider> UserLoginProvides { get; set; }

        #endregion

        #region Content

        public virtual DbSet<Content> Contents { get; set; }

        public virtual DbSet<ContentBoilerplate> ContentBoilerplates { get; set; }

        public virtual DbSet<ContentCategory> ContentCategories { get; set; }

        public virtual DbSet<ContentFeedback> ContentFeedbacks { get; set; }

        public virtual DbSet<ContentUrl> ContentUrls { get; set; }

        public virtual DbSet<ContentVersion> ContentVersions { get; set; }

        public virtual DbSet<TrackingSource> TrackingSources { get; set; }

        public virtual DbSet<ContentTrackingSource> ContentTrackingSources { get; set; }

        public virtual DbSet<Feedback> Feedbacks { get; set; }

        public virtual DbSet<ToDo> ToDos { get; set; }

        public virtual DbSet<LexiconEntry> Lexicon { get; set; }

        #endregion

        #region UserAdmin

        public virtual DbSet<UserNote> UserNotes { get; set; }

        public virtual DbSet<UserAdvertising> UserAdvertising { get; set; }

        public virtual DbSet<UserAdvertisingForm> UserAdvertisingForms { get; set; }

        public virtual DbSet<UserNoteAction> UserNoteActions { get; set; }

        public virtual DbSet<UserMultiaccount> UserMultiaccounts { get; set; }

        public virtual DbSet<RoleChainEntry> RoleChain { get; set; }

        #endregion

        #region Calendar

        public virtual DbSet<Appointment> Appointments { get; set; }

        public virtual DbSet<AppointmentArea> AppointmentAreas { get; set; }

        public virtual DbSet<AppointmentNotification> AppointmentNotifications { get; set; }

        public virtual DbSet<AppointmentSentNotification> AppointmentSentNotifications { get; set; }

        public virtual DbSet<AppointmentParticipation> AppointmentParticipation { get; set; }

        public virtual DbSet<AppointmentType> AppointmentType { get; set; }

        #endregion

        #region Fanart

        public virtual DbSet<Fanart> Fanarts { get; set; }

        public virtual DbSet<FanartCategory> FanartCategories { get; set; }

        public virtual DbSet<FanartFavorite> FanartFavorites { get; set; }

        public virtual DbSet<FanartTags> FanartsTags { get; set; }

        public virtual DbSet<FanartTag> FanartTags { get; set; }

        public virtual DbSet<FanartBanlist> FanartBanlist { get; set; }

        public virtual DbSet<FanartChallenge> FanartChallenges { get; set; }

        public virtual DbSet<FanartChallengeVote> FanartChallengeVotes { get; set; }

        public virtual DbSet<FanartRating> FanartRatings { get; set; }

        #endregion

        #region Friendcode

        public virtual DbSet<Friendcode> Friendcodes { get; set; }

        public virtual DbSet<FriendcodeGame> FriendcodeGames { get; set; }

        #endregion

        #region Pokedex

        public virtual DbSet<Ability> Abilities { get; set; }

        public virtual DbSet<Attack> Attacks { get; set; }

        public virtual DbSet<AttackHiddenMachine> AttackHiddenMachines { get; set; }

        public virtual DbSet<AttackTarget> AttackTargets { get; set; }

        public virtual DbSet<AttackTargetType> AttackTargetTypes { get; set; }

        public virtual DbSet<AttackTechnicalMachine> AttackTechnicalMachines { get; set; }

        public virtual DbSet<Berry> Berries { get; set; }

        public virtual DbSet<Item> Items { get; set; }

        public virtual DbSet<ItemLocation> ItemLocations { get; set; }

        public virtual DbSet<Location> Locations { get; set; }

        public virtual DbSet<Nature> Natures { get; set; }

        public virtual DbSet<Pokeball> Pokeballs { get; set; }

        public virtual DbSet<Pokemon> Pokemon { get; set; }

        public virtual DbSet<PokemonAbility> PokemonAbilities { get; set; }

        public virtual DbSet<PokemonAttack> PokemonAttacks { get; set; }

        public virtual DbSet<PokemonBodyForm> PokemonBodyForms { get; set; }

        public virtual DbSet<PokemonColor> PokemonColors { get; set; }

        public virtual DbSet<PokemonContestType> PokemonContestTypes { get; set; }

        public virtual DbSet<PokemonEdition> PokemonEditions { get; set; }

        public virtual DbSet<PokemonEditionGroup> PokemonEditionGroups { get; set; }

        public virtual DbSet<PokemonFootprint> PokemonFootprints { get; set; }

        public virtual DbSet<PokemonImage> PokemonImages { get; set; }

        public virtual DbSet<PokemonItem> PokemonItems { get; set; }

        public virtual DbSet<PokemonLocation> PokemonLocations { get; set; }

        public virtual DbSet<PokemonLocationMethod> PokemonLocationMethods { get; set; }

        public virtual DbSet<PokemonPokedexDescription> PokemonPokedexDescriptions { get; set; }

        public virtual DbSet<PokemonStrategy> PokemonStrategies { get; set; }

        public virtual DbSet<PokemonTier> PokemonTiers { get; set; }

        public virtual DbSet<PokemonTiers> PokemonTiersJointable { get; set; }

        public virtual DbSet<PokemonType> PokemonTypes { get; set; }

        public virtual DbSet<PokemonTypeEffectivity> PokemonTypeEffectivities { get; set; }

        public virtual DbSet<PokePower> PokePowers { get; set; }

        public virtual DbSet<RangerAbility> RangerAbilities { get; set; }

        public virtual DbSet<RangerLocation> RangerLocations { get; set; }

        public virtual DbSet<RangerPokemon> RangerPokemon { get; set; }

        public virtual DbSet<RangerPokemonLocation> RangerPokemonLocations { get; set; }

        public virtual DbSet<MysteryDungeonReach> MysteryDungeonReaches { get; set; }

        #endregion

        #region Service

        public virtual DbSet<FriendSafari> FriendSafaris { get; set; }

        public virtual DbSet<QrCode> QrCodes { get; set; }

        #endregion

        #region Strategy

        public virtual DbSet<Counter> Counters { get; set; }

        public virtual DbSet<Metagame> Metagames { get; set; }

        public virtual DbSet<MetagameVersus> MetagameVersi { get; set; }

        public virtual DbSet<Moveset> Movesets { get; set; }

        public virtual DbSet<Tournament> Tournament { get; set; }

        public virtual DbSet<TournamentClass> TournamentClasses { get; set; }

        public virtual DbSet<TournamentParticipation> TournamentParticipation { get; set; }

        #endregion

        #region Tracker

        public virtual DbSet<ChatAppletTracker> ChatAppletTracker { get; set; }

        public virtual DbSet<RefLinkTracker> RefLinkTracker { get; set; }

        public virtual DbSet<SearchTracker> SearchTracker { get; set; }

        public virtual DbSet<UserAdminTracker> UserAdminTracker { get; set; }

        #endregion

        #region UserData

        public virtual DbSet<UserActivity> UserActivities { get; set; }

        public virtual DbSet<UserEmployeeOfTheMonth> UserEmployeesOfTheMonth { get; set; }

        public virtual DbSet<UserNotification> UserNotifications { get; set; }

        public virtual DbSet<UserPage> UserPages { get; set; }

        public virtual DbSet<UserProfile> UserProfile { get; set; }

        public virtual DbSet<UserUpload> UserUploads { get; set; }

        public virtual DbSet<PrivateMessage> PrivateMessages { get; set; }

        public virtual DbSet<PrivateMessageInbox> PrivateMessagesInbox { get; set; }

        public virtual DbSet<PrivateMessageInboxLabel> PrivateMessagesInboxLabels { get; set; }

        public virtual DbSet<PrivateMessageLabel> PrivateMessageLabels { get; set; }

        public virtual DbSet<PrivateMessageSent> PrivateMessagesSent { get; set; }

        public virtual DbSet<PrivateMessageSentLabel> PrivateMessagesSentLabels { get; set; }

        public virtual DbSet<UserFeedConfig> UserFeedConfigs { get; set; }

        public virtual DbSet<UserFollower> UserFollowers { get; set; }

        #endregion

        #region Wifi

        public virtual DbSet<Offer> WifiOffers { get; set; }

        public virtual DbSet<Interest> WifiInterests { get; set; }

        public virtual DbSet<TradeLog> TradeLogs { get; set; }

        public virtual DbSet<WifiBanlist> WifiBanlist { get; set; }

        #endregion

        #region Comments
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<CommentAncestor> CommentAncestors { get; set; }
        #endregion

        #region Forum

        public virtual DbSet<Board> Boards { get; set; }

        public virtual DbSet<Post> Post { get; set; }

        public virtual DbSet<Thread> Thread { get; set; }

        public virtual DbSet<UnreadForumTracker> UnreadForumTracker { get; set; }

        public virtual DbSet<UnreadThreadTracker> UnreadThreadTracker { get; set; }

        public virtual DbSet<BoardPermissions> BoardPermissions { get; set; }

        public virtual DbSet<ForumGroup> ForumGroups { get; set; }

        public virtual DbSet<ForumGroupsUsers> ForumGroupsUsers { get; set; }

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(User.MiniAvatarFileNameExpression);

            modelBuilder.Entity<FanartTag>().ToTable("fanart_tags");
            modelBuilder.Entity<FanartTags>().ToTable("fanarts_tags");

            Ability.OnModelCreating(modelBuilder);
            Attack.OnModelCreating(modelBuilder);
            Berry.OnModelCreating(modelBuilder);
            Pokefans.Data.Pokedex.Pokemon.OnModelCreating(modelBuilder);
            Offer.OnModelCreating(modelBuilder);
            Item.OnModelCreating(modelBuilder);
            Fanart.OnModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
