namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OldDbPort : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.dex_ability",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name_german = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_english = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_japanese = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_korean = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_french = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_chinese = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_japanese_trans = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_korean_trans = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_chinese_trans = c.String(maxLength: 100, storeType: "nvarchar"),
                        IngameDescription = c.String(maxLength: 255, storeType: "nvarchar"),
                        IngameDescriptionEnglish = c.String(maxLength: 255, storeType: "nvarchar"),
                        Abstract = c.String(unicode: false),
                        AbstractCode = c.String(unicode: false),
                        BattleDescription = c.String(unicode: false),
                        BattleDescriptionCode = c.String(unicode: false),
                        ExternalDescription = c.String(unicode: false),
                        ExternalDescriptionCode = c.String(unicode: false),
                        MysteryDungeonDescription = c.String(unicode: false),
                        MysteryDungeonDescriptionCode = c.String(unicode: false),
                        Generation = c.Int(nullable: false),
                        EmeraldDescription = c.String(unicode: false),
                        EmeraldDescriptionCode = c.String(unicode: false),
                        LastUpdateTime = c.DateTime(nullable: false, precision: 0),
                        LastUpdateUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_users", t => t.LastUpdateUserId, cascadeDelete: true)
                .Index(t => t.LastUpdateUserId);
            
            CreateTable(
                "dbo.AppointmentAreas",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.AppointmentNotifications",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        EventTypeId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Days = c.Int(nullable: false),
                        AppointmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Appointments", t => t.AppointmentId, cascadeDelete: true)
                .ForeignKey("dbo.AppointmentTypes", t => t.EventTypeId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.EventTypeId)
                .Index(t => t.UserId)
                .Index(t => t.AppointmentId);
            
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Begin = c.DateTime(nullable: false, precision: 0),
                        End = c.DateTime(nullable: false, precision: 0),
                        AppointmentTypeId = c.Int(nullable: false),
                        Name = c.String(maxLength: 200, storeType: "nvarchar"),
                        Description = c.String(unicode: false),
                        DescriptionCode = c.String(unicode: false),
                        Teaser = c.String(maxLength: 250, storeType: "nvarchar"),
                        DisplayInForum = c.Boolean(nullable: false),
                        Url = c.String(maxLength: 100, storeType: "nvarchar"),
                        CanParticipate = c.Boolean(nullable: false),
                        IsHidden = c.Boolean(nullable: false),
                        AuthorId = c.Int(nullable: false),
                        EditorId = c.Int(nullable: false),
                        LastEditTime = c.DateTime(nullable: false, precision: 0),
                        CreationTime = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AppointmentTypes", t => t.AppointmentTypeId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.EditorId, cascadeDelete: true)
                .Index(t => t.Begin)
                .Index(t => t.End)
                .Index(t => t.AppointmentTypeId)
                .Index(t => t.DisplayInForum)
                .Index(t => t.Url)
                .Index(t => t.IsHidden)
                .Index(t => t.AuthorId)
                .Index(t => t.EditorId);
            
            CreateTable(
                "dbo.AppointmentTypes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.AppointmentParticipations",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        AppointmentId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Appointments", t => t.AppointmentId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.AppointmentId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AppointmentSentNotifications",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        AppointmentId = c.Int(nullable: false),
                        NotificationTime = c.DateTime(nullable: false, precision: 0),
                        NotificationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Appointments", t => t.AppointmentId, cascadeDelete: true)
                .ForeignKey("dbo.AppointmentNotifications", t => t.NotificationId, cascadeDelete: true)
                .Index(t => t.AppointmentId)
                .Index(t => t.NotificationId);
            
            CreateTable(
                "dbo.AttackHiddenMachines",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        AttackId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        Generation = c.Int(nullable: false),
                        AttackHiddenMachineId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Attacks", t => t.AttackId, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.AttackHiddenMachines", t => t.AttackHiddenMachineId)
                .Index(t => t.AttackId)
                .Index(t => t.ItemId)
                .Index(t => t.AttackHiddenMachineId);
            
            CreateTable(
                "dbo.Attacks",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Url = c.String(unicode: false),
                        name_german = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_english = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_japanese = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_korean = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_french = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_chinese = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_japanese_trans = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_korean_trans = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_chinese_trans = c.String(maxLength: 100, storeType: "nvarchar"),
                        Generation = c.Int(nullable: false),
                        TypeId = c.Int(nullable: false),
                        Mode = c.Int(nullable: false),
                        Damage = c.Int(nullable: false),
                        AttackPoints = c.Int(nullable: false),
                        Accuracy = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Priority = c.Int(nullable: false),
                        Description = c.String(unicode: false),
                        DecriptionCode = c.String(unicode: false),
                        flag_contact = c.Boolean(nullable: false),
                        flag_bite = c.Boolean(nullable: false),
                        flag_punch = c.Boolean(nullable: false),
                        flag_sound = c.Boolean(nullable: false),
                        flag_puls = c.Boolean(nullable: false),
                        flag_recoil = c.Boolean(nullable: false),
                        flag_recoil_amount = c.Short(nullable: false),
                        flag_self_heal = c.Boolean(nullable: false),
                        flag_heal_is_absorbing = c.Boolean(nullable: false),
                        flag_heal_amount = c.Short(nullable: false),
                        flag_change_damage = c.Boolean(nullable: false),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                        UpdateUserId = c.Int(nullable: false),
                        ContestTypeId = c.Int(nullable: false),
                        ContestAppeal = c.Byte(nullable: false),
                        ContestJam = c.Byte(nullable: false),
                        ContestDescription = c.String(maxLength: 200, storeType: "nvarchar"),
                        DungeonAccuracy = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DungeonCritical = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MysteryDungeonReachId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.PokemonContestTypes", t => t.ContestTypeId, cascadeDelete: true)
                .ForeignKey("dbo.dex_types", t => t.TypeId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.UpdateUserId, cascadeDelete: true)
                .Index(t => t.TypeId)
                .Index(t => t.UpdateUserId)
                .Index(t => t.ContestTypeId);
            
            CreateTable(
                "dbo.AttackTargets",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        AttackId = c.Int(nullable: false),
                        AttackTargetTypeId = c.Int(nullable: false),
                        Generation = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AttackTargetTypes", t => t.AttackTargetTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Attacks", t => t.AttackId, cascadeDelete: true)
                .Index(t => t.AttackId)
                .Index(t => t.AttackTargetTypeId);
            
            CreateTable(
                "dbo.AttackTargetTypes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.AttackTechnicalMachines",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        AttackId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        Generation = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.Attacks", t => t.AttackId, cascadeDelete: true)
                .Index(t => t.AttackId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Url = c.String(maxLength: 25, storeType: "nvarchar"),
                        name_german = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_english = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_japanese = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_korean = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_french = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_chinese = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_japanese_trans = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_korean_trans = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_chinese_trans = c.String(maxLength: 100, storeType: "nvarchar"),
                        Description = c.String(unicode: false),
                        DescriptionCode = c.String(unicode: false),
                        IngameDescription = c.String(maxLength: 255, storeType: "nvarchar"),
                        ImageUrl = c.String(maxLength: 25, storeType: "nvarchar"),
                        UpdatedOn = c.DateTime(nullable: false, precision: 0),
                        UpdateUserId = c.Int(nullable: false),
                        RequestCount = c.Int(nullable: false),
                        SearchCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_users", t => t.UpdateUserId, cascadeDelete: true)
                .Index(t => t.UpdateUserId);
            
            CreateTable(
                "dbo.PokemonContestTypes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Class = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.dex_types",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        CodeHandle = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Berries",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        IngameId = c.Int(nullable: false),
                        name_german = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_english = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_japanese = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_korean = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_french = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_chinese = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_japanese_trans = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_korean_trans = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_chinese_trans = c.String(maxLength: 100, storeType: "nvarchar"),
                        NameOrigin = c.String(maxLength: 100, storeType: "nvarchar"),
                        AttackTypeId = c.Int(nullable: false),
                        AttackDamage = c.Int(nullable: false),
                        Quality = c.Int(nullable: false),
                        Texture = c.Int(nullable: false),
                        Size = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HarvestMin = c.Int(nullable: false),
                        HarvestMax = c.Int(nullable: false),
                        GrowTime = c.Int(nullable: false),
                        WateringTime = c.Int(nullable: false),
                        Spicy = c.Int(nullable: false),
                        Dry = c.Int(nullable: false),
                        Sweet = c.Int(nullable: false),
                        Bitter = c.Int(nullable: false),
                        Sour = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.dex_types", t => t.AttackTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.AttackTypeId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.ChatAppletTrackers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false, precision: 0),
                        Ip = c.String(maxLength: 39, storeType: "nvarchar"),
                        Host = c.String(maxLength: 255, storeType: "nvarchar"),
                        Nickname = c.String(maxLength: 30, storeType: "nvarchar"),
                        AppletName = c.String(maxLength: 10, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Counters",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        PokemonId = c.Int(nullable: false),
                        CounterId = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 1024, storeType: "nvarchar"),
                        RemarksCode = c.String(maxLength: 768, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.dex_pokemon", t => t.CounterId, cascadeDelete: true)
                .ForeignKey("dbo.dex_pokemon", t => t.PokemonId, cascadeDelete: true)
                .Index(t => t.PokemonId)
                .Index(t => t.CounterId);
            
            CreateTable(
                "dbo.dex_pokemon",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        id_national = c.Int(nullable: false),
                        id_kanto = c.Int(),
                        id_johto = c.Int(),
                        id_hoenn = c.Int(),
                        id_sinnoh = c.Int(),
                        id_einall = c.Int(),
                        id_kalos = c.Int(),
                        id_ranger1 = c.Int(),
                        id_ranger2 = c.Int(),
                        id_ranger3 = c.Int(),
                        id_dungeon1 = c.Int(),
                        id_dungeon2 = c.Int(),
                        id_dungeon3 = c.Int(),
                        url = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_german = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_english = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_japanese = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_korean = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_french = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_chinese = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_japanese_trans = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_korean_trans = c.String(maxLength: 100, storeType: "nvarchar"),
                        name_chinese_trans = c.String(maxLength: 100, storeType: "nvarchar"),
                        Generation = c.Int(nullable: false),
                        Description = c.String(unicode: false),
                        DescriptionCode = c.String(unicode: false),
                        Abstract = c.String(unicode: false),
                        Trivia = c.String(unicode: false),
                        TriviaCode = c.String(unicode: false),
                        NameDescription = c.String(unicode: false),
                        Type1 = c.Int(nullable: false),
                        Type2 = c.Int(),
                        EvolutionBaseId = c.Int(),
                        EvolutionItemId = c.Int(),
                        EvolutionLevel = c.Int(),
                        EvolutionDescription = c.String(unicode: false),
                        EvolutionParentId = c.Int(),
                        GenderProbabilityFemale = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GenderProbabilityMale = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CanBreed = c.Boolean(nullable: false),
                        BreedingGroup1Id = c.Int(),
                        BreedingGroup2Id = c.Int(),
                        BreedingSteps = c.Int(),
                        value_hp = c.Int(nullable: false),
                        value_attack = c.Int(nullable: false),
                        value_defense = c.Int(nullable: false),
                        value_special_attack = c.Int(nullable: false),
                        value_special_defense = c.Int(nullable: false),
                        value_speed = c.Int(nullable: false),
                        base_hp = c.Int(nullable: false),
                        base_attack = c.Int(nullable: false),
                        base_defense = c.Int(nullable: false),
                        base_special_attack = c.Int(nullable: false),
                        base_special_defense = c.Int(nullable: false),
                        base_speed = c.Int(nullable: false),
                        ev_hp = c.Int(nullable: false),
                        ev_attack = c.Int(nullable: false),
                        ev_defense = c.Int(nullable: false),
                        ev_special_attack = c.Int(nullable: false),
                        ev_special_defense = c.Int(nullable: false),
                        ev_speed = c.Int(nullable: false),
                        pokeathlon_speed = c.Decimal(nullable: false, precision: 18, scale: 2),
                        pokeathlon_force = c.Decimal(nullable: false, precision: 18, scale: 2),
                        pokeathlon_sustain = c.Decimal(nullable: false, precision: 18, scale: 2),
                        pokeathlon_technique = c.Decimal(nullable: false, precision: 18, scale: 2),
                        pokeathlon_jump = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Species = c.String(maxLength: 50, storeType: "nvarchar"),
                        Size = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ColorId = c.Int(nullable: false),
                        FootprintId = c.Int(nullable: false),
                        BodyFormId = c.Int(nullable: false),
                        EpWin = c.Int(nullable: false),
                        EpMax = c.Int(nullable: false),
                        CatchRate = c.Int(nullable: false),
                        Friendship = c.Int(nullable: false),
                        InPokemonConquest = c.Boolean(nullable: false),
                        ForumTopicId = c.Int(nullable: false),
                        FormItemNeededId = c.Int(),
                        FormChangeDescription = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.dex_bodyforms", t => t.BodyFormId, cascadeDelete: true)
                .ForeignKey("dbo.dex_breedinggroups", t => t.BreedingGroup1Id)
                .ForeignKey("dbo.dex_breedinggroups", t => t.BreedingGroup2Id)
                .ForeignKey("dbo.dex_color", t => t.ColorId, cascadeDelete: true)
                .ForeignKey("dbo.dex_pokemon", t => t.EvolutionBaseId)
                .ForeignKey("dbo.Items", t => t.EvolutionItemId)
                .ForeignKey("dbo.dex_pokemon", t => t.EvolutionParentId)
                .ForeignKey("dbo.dex_footprints", t => t.FootprintId, cascadeDelete: true)
                .Index(t => t.url, unique: true)
                .Index(t => t.EvolutionBaseId)
                .Index(t => t.EvolutionItemId)
                .Index(t => t.EvolutionParentId)
                .Index(t => t.BreedingGroup1Id)
                .Index(t => t.BreedingGroup2Id)
                .Index(t => t.ColorId)
                .Index(t => t.FootprintId)
                .Index(t => t.BodyFormId);
            
            CreateTable(
                "dbo.dex_bodyforms",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        ImageUrl = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.dex_breedinggroups",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.dex_color",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.dex_footprints",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        ImageUrl = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.PokemonAbilities",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        PokemonId = c.Int(nullable: false),
                        AbilityId = c.Int(nullable: false),
                        IsHidden = c.Boolean(nullable: false),
                        IsMega = c.Boolean(nullable: false),
                        IsDreamworld = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.dex_ability", t => t.AbilityId, cascadeDelete: true)
                .ForeignKey("dbo.dex_pokemon", t => t.PokemonId, cascadeDelete: true)
                .Index(t => t.PokemonId)
                .Index(t => t.AbilityId);
            
            CreateTable(
                "dbo.dex_strategy",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Generation = c.Int(nullable: false),
                        PokemonId = c.Int(nullable: false),
                        Text = c.String(unicode: false),
                        Code = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.dex_pokemon", t => t.PokemonId, cascadeDelete: true)
                .Index(t => t.PokemonId);
            
            CreateTable(
                "dbo.FanartCategories",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.FanartFavorites",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        FanartId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Fanarts", t => t.FanartId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.FanartId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Fanarts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        Url = c.String(maxLength: 255, storeType: "nvarchar"),
                        Title = c.String(maxLength: 100, storeType: "nvarchar"),
                        Description = c.String(unicode: false),
                        DescriptionCode = c.String(unicode: false),
                        Rating = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RatingCount = c.Int(nullable: false),
                        SmartRating = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ForceComments = c.Boolean(nullable: false),
                        IsTileset = c.Boolean(nullable: false),
                        Protect = c.Boolean(nullable: false),
                        PokemonId = c.Int(),
                        UploadTime = c.DateTime(nullable: false, precision: 0),
                        UploadUserId = c.Int(nullable: false),
                        UploadIp = c.String(maxLength: 47, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.FanartCategories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.dex_pokemon", t => t.PokemonId)
                .Index(t => t.CategoryId)
                .Index(t => t.PokemonId);
            
            CreateTable(
                "dbo.FanartTags",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        FanartId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Fanarts", t => t.FanartId, cascadeDelete: true)
                .ForeignKey("dbo.FanartTags1", t => t.TagId, cascadeDelete: true)
                .Index(t => t.FanartId)
                .Index(t => t.TagId);
            
            CreateTable(
                "dbo.FanartTags1",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.FriendcodeGames",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255, storeType: "nvarchar"),
                        Url = c.String(maxLength: 255, storeType: "nvarchar"),
                        Category = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Friendcodes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        GameId = c.Int(nullable: false),
                        FriendCode = c.String(maxLength: 16, storeType: "nvarchar"),
                        IngameName = c.String(unicode: false),
                        IsPublic = c.Boolean(nullable: false),
                        LastUpate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.FriendcodeGames", t => t.GameId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.GameId);
            
            CreateTable(
                "dbo.FriendSafaris",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        FriendCodeId = c.Int(nullable: false),
                        IngameName = c.String(unicode: false),
                        PokemonTypeId = c.Int(nullable: false),
                        Pokemon1Id = c.Int(nullable: false),
                        Pokemon2Id = c.Int(nullable: false),
                        Pokemon3Id = c.Int(nullable: false),
                        Annotations = c.String(unicode: false),
                        LastUpdate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Friendcodes", t => t.FriendCodeId, cascadeDelete: true)
                .ForeignKey("dbo.dex_pokemon", t => t.Pokemon1Id, cascadeDelete: true)
                .ForeignKey("dbo.dex_pokemon", t => t.Pokemon2Id, cascadeDelete: true)
                .ForeignKey("dbo.dex_pokemon", t => t.Pokemon3Id, cascadeDelete: true)
                .ForeignKey("dbo.dex_types", t => t.PokemonTypeId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.FriendCodeId)
                .Index(t => t.PokemonTypeId)
                .Index(t => t.Pokemon1Id)
                .Index(t => t.Pokemon2Id)
                .Index(t => t.Pokemon3Id);
            
            CreateTable(
                "dbo.ItemLocations",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        LocationId = c.Int(nullable: false),
                        Description = c.String(unicode: false),
                        EditionId = c.Int(nullable: false),
                        PokemonEditionGroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.PokemonEditions", t => t.EditionId, cascadeDelete: true)
                .ForeignKey("dbo.PokemonEditionGroups", t => t.PokemonEditionGroupId, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.LocationId)
                .Index(t => t.EditionId)
                .Index(t => t.PokemonEditionGroupId);
            
            CreateTable(
                "dbo.PokemonEditions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Generation = c.Int(nullable: false),
                        Name = c.String(unicode: false),
                        PokemonEditionGroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.PokemonEditionGroups", t => t.PokemonEditionGroupId, cascadeDelete: true)
                .Index(t => t.PokemonEditionGroupId);
            
            CreateTable(
                "dbo.PokemonEditionGroups",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Markup = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.LexiconEntries",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 100, storeType: "nvarchar"),
                        Url = c.String(maxLength: 100, storeType: "nvarchar"),
                        Description = c.String(unicode: false),
                        DescriptionCode = c.String(unicode: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Metagames",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.MetagameVersus",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Movesets",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        PokemonId = c.Int(nullable: false),
                        Generation = c.Int(nullable: false),
                        Title = c.String(maxLength: 100, storeType: "nvarchar"),
                        Set = c.String(unicode: false),
                        SetCode = c.String(unicode: false),
                        Description = c.String(unicode: false),
                        DescriptionCode = c.String(unicode: false),
                        Classification = c.Int(),
                        Status = c.Int(nullable: false),
                        RejectReason = c.String(unicode: false),
                        ApprovalUserId = c.Int(nullable: false),
                        ApprovalTime = c.DateTime(nullable: false, precision: 0),
                        AuthorUserId = c.Int(),
                        AuthorIp = c.String(maxLength: 46, storeType: "nvarchar"),
                        SubmissionTime = c.DateTime(nullable: false, precision: 0),
                        AuthorUserAgent = c.String(unicode: false),
                        AuthorTiming = c.Int(nullable: false),
                        UpdateUserId = c.Int(nullable: false),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_users", t => t.ApprovalUserId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.AuthorUserId)
                .ForeignKey("dbo.dex_pokemon", t => t.PokemonId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.UpdateUserId, cascadeDelete: true)
                .Index(t => t.PokemonId)
                .Index(t => t.ApprovalUserId)
                .Index(t => t.AuthorUserId)
                .Index(t => t.UpdateUserId);
            
            CreateTable(
                "dbo.MysteryDungeonReaches",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Display = c.String(unicode: false),
                        ImageName = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Natures",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Effect = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Pokeballs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100, storeType: "nvarchar"),
                        ImagePath = c.String(maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.PokemonAttacks",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        EditionId = c.Int(nullable: false),
                        PokemonId = c.Int(nullable: false),
                        AttackId = c.Int(nullable: false),
                        Details = c.String(unicode: false),
                        DetailsCode = c.String(unicode: false),
                        EventName = c.String(unicode: false),
                        GameName = c.String(unicode: false),
                        Level = c.Int(),
                        AttackTechnicalMachineId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Attacks", t => t.AttackId, cascadeDelete: true)
                .ForeignKey("dbo.PokemonEditions", t => t.EditionId, cascadeDelete: true)
                .ForeignKey("dbo.dex_pokemon", t => t.PokemonId, cascadeDelete: true)
                .ForeignKey("dbo.AttackTechnicalMachines", t => t.AttackTechnicalMachineId, cascadeDelete: true)
                .Index(t => t.EditionId)
                .Index(t => t.PokemonId)
                .Index(t => t.AttackId)
                .Index(t => t.AttackTechnicalMachineId);
            
            CreateTable(
                "dbo.PokemonImages",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        PokemonId = c.Int(nullable: false),
                        FilePath = c.String(maxLength: 255, storeType: "nvarchar"),
                        Description = c.String(unicode: false),
                        UploadUserId = c.Int(nullable: false),
                        UploadTime = c.DateTime(nullable: false, precision: 0),
                        OriginalFileName = c.String(maxLength: 255, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.dex_pokemon", t => t.PokemonId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.UploadUserId, cascadeDelete: true)
                .Index(t => t.PokemonId)
                .Index(t => t.UploadUserId);
            
            CreateTable(
                "dbo.PokemonItems",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        PokemonId = c.Int(nullable: false),
                        EditionId = c.Int(nullable: false),
                        EditionGroupId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        Hint = c.String(maxLength: 255, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.PokemonEditions", t => t.EditionId, cascadeDelete: true)
                .ForeignKey("dbo.PokemonEditionGroups", t => t.EditionGroupId, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.dex_pokemon", t => t.PokemonId, cascadeDelete: true)
                .Index(t => t.PokemonId)
                .Index(t => t.EditionId)
                .Index(t => t.EditionGroupId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.PokemonLocationMethods",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.PokemonLocations",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        PokemonId = c.Int(nullable: false),
                        LocationMethodId = c.Int(nullable: false),
                        Commonness = c.Int(nullable: false),
                        LevelMin = c.Int(nullable: false),
                        LevelMax = c.Int(),
                        Description = c.String(unicode: false),
                        DescriptionCode = c.String(unicode: false),
                        EditionId = c.Int(nullable: false),
                        LastUpdate = c.DateTime(nullable: false, precision: 0),
                        UpdateUserId = c.Int(nullable: false),
                        Time = c.Int(nullable: false),
                        Season = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.PokemonEditions", t => t.EditionId, cascadeDelete: true)
                .ForeignKey("dbo.PokemonLocationMethods", t => t.LocationMethodId, cascadeDelete: true)
                .ForeignKey("dbo.dex_pokemon", t => t.PokemonId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.UpdateUserId, cascadeDelete: true)
                .Index(t => t.PokemonId)
                .Index(t => t.LocationMethodId)
                .Index(t => t.EditionId)
                .Index(t => t.UpdateUserId);
            
            CreateTable(
                "dbo.PokemonPokedexDescriptions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        EditionId = c.Int(nullable: false),
                        Description = c.String(unicode: false),
                        DescriptionEnglish = c.String(unicode: false),
                        PokemonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.PokemonEditions", t => t.EditionId, cascadeDelete: true)
                .ForeignKey("dbo.dex_pokemon", t => t.PokemonId, cascadeDelete: true)
                .Index(t => t.EditionId)
                .Index(t => t.PokemonId);
            
            CreateTable(
                "dbo.dex_tiers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        ShortName = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.dex_pokemon_tiers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        PokemonId = c.Int(nullable: false),
                        TierId = c.Int(nullable: false),
                        Generation = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.dex_pokemon", t => t.PokemonId, cascadeDelete: true)
                .ForeignKey("dbo.dex_tiers", t => t.TierId, cascadeDelete: true)
                .Index(t => t.PokemonId)
                .Index(t => t.TierId);
            
            CreateTable(
                "dbo.PokemonTypeEffectivities",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Type1Id = c.Int(nullable: false),
                        Type2Id = c.Int(nullable: false),
                        Relation = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.dex_types", t => t.Type1Id, cascadeDelete: true)
                .ForeignKey("dbo.dex_types", t => t.Type2Id, cascadeDelete: true)
                .Index(t => t.Type1Id)
                .Index(t => t.Type2Id);
            
            CreateTable(
                "dbo.PokePowers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.QrCodes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        FriendCodeId = c.Int(nullable: false),
                        IngameName = c.String(unicode: false),
                        LocationId = c.Int(nullable: false),
                        Slot = c.String(unicode: false),
                        LastUpate = c.DateTime(nullable: false, precision: 0),
                        ImageName = c.String(unicode: false),
                        OriginalImageName = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Friendcodes", t => t.FriendCodeId, cascadeDelete: true)
                .ForeignKey("dbo.PokemonLocations", t => t.LocationId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.FriendCodeId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.RangerAbilities",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255, storeType: "nvarchar"),
                        ImagePath = c.String(maxLength: 255, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.RangerLocations",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.RangerPokemons",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        PokemonId = c.Int(nullable: false),
                        TypeId = c.Int(nullable: false),
                        PokePowerId = c.Int(nullable: false),
                        RangerAbilityId = c.Int(nullable: false),
                        AbilityPower = c.Int(nullable: false),
                        Circles = c.Int(nullable: false),
                        RangerVersion = c.Int(nullable: false),
                        EscapeValue = c.Int(nullable: false),
                        Experience = c.Int(nullable: false),
                        CatchHint = c.String(unicode: false),
                        CatchHintCode = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.RangerAbilities", t => t.RangerAbilityId, cascadeDelete: true)
                .ForeignKey("dbo.dex_types", t => t.TypeId, cascadeDelete: true)
                .ForeignKey("dbo.dex_pokemon", t => t.PokemonId, cascadeDelete: true)
                .ForeignKey("dbo.PokePowers", t => t.PokePowerId, cascadeDelete: true)
                .Index(t => t.PokemonId)
                .Index(t => t.TypeId)
                .Index(t => t.PokePowerId)
                .Index(t => t.RangerAbilityId);
            
            CreateTable(
                "dbo.RangerPokemonLocations",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        RangerPokemonId = c.Int(nullable: false),
                        RangerVersion = c.Int(nullable: false),
                        IsStoryLocation = c.Boolean(nullable: false),
                        Description = c.String(unicode: false),
                        DescriptionCode = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.RefLinkTrackers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false, precision: 0),
                        Ip = c.String(maxLength: 39, storeType: "nvarchar"),
                        PartnerName = c.String(maxLength: 100, storeType: "nvarchar"),
                        Parameter = c.String(maxLength: 255, storeType: "nvarchar"),
                        ParameterValue = c.String(maxLength: 100, storeType: "nvarchar"),
                        SourceUrl = c.String(maxLength: 255, storeType: "nvarchar"),
                        SourceCampaign = c.String(maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ReviewIndexes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Url = c.String(maxLength: 255, storeType: "nvarchar"),
                        Name = c.String(maxLength: 255, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ReviewItems",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Url = c.String(maxLength: 255, storeType: "nvarchar"),
                        Name = c.String(maxLength: 255, storeType: "nvarchar"),
                        Rating = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RatingCount = c.Int(nullable: false),
                        Requests = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 255, storeType: "nvarchar"),
                        Rating = c.Byte(nullable: false),
                        Text = c.String(unicode: false),
                        TextCode = c.String(unicode: false),
                        CreatorIp = c.String(unicode: false),
                        CreationTime = c.DateTime(nullable: false, precision: 0),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                        UserId = c.Int(nullable: false),
                        ReviewItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.ReviewItems", t => t.ReviewItemId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ReviewItemId);
            
            CreateTable(
                "dbo.SearchTrackers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false, precision: 0),
                        Ip = c.String(maxLength: 39, storeType: "nvarchar"),
                        Input = c.String(maxLength: 255, storeType: "nvarchar"),
                        InputUri = c.String(maxLength: 255, storeType: "nvarchar"),
                        Index = c.String(maxLength: 10, storeType: "nvarchar"),
                        Source = c.String(maxLength: 100, storeType: "nvarchar"),
                        Visitor = c.String(maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Tournaments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        MetagameId = c.Int(nullable: false),
                        TierId = c.Int(nullable: false),
                        MetagameVersusId = c.Int(nullable: false),
                        TournamentClassId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.TournamentClasses", t => t.TournamentClassId, cascadeDelete: true)
                .ForeignKey("dbo.Metagames", t => t.MetagameId, cascadeDelete: true)
                .ForeignKey("dbo.dex_tiers", t => t.TierId, cascadeDelete: true)
                .ForeignKey("dbo.MetagameVersus", t => t.MetagameVersusId, cascadeDelete: true)
                .Index(t => t.MetagameId)
                .Index(t => t.TierId)
                .Index(t => t.MetagameVersusId)
                .Index(t => t.TournamentClassId);
            
            CreateTable(
                "dbo.TournamentClasses",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        DisplayCode = c.String(unicode: false),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.TournamentParticipations",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        TournamentId = c.Int(nullable: false),
                        Place = c.Int(nullable: false),
                        Points = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Tournaments", t => t.TournamentId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.TournamentId);
            
            CreateTable(
                "dbo.TradeLogs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Price = c.Int(nullable: false),
                        PokemonId = c.Int(nullable: false),
                        OfferId = c.Int(nullable: false),
                        CompletedTime = c.DateTime(nullable: false, precision: 0),
                        ValidOn = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Offers", t => t.OfferId, cascadeDelete: true)
                .ForeignKey("dbo.dex_pokemon", t => t.PokemonId, cascadeDelete: true)
                .Index(t => t.PokemonId)
                .Index(t => t.OfferId);
            
            CreateTable(
                "dbo.Offers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ItemId = c.Int(),
                        PokemonId = c.Int(),
                        Level = c.Int(nullable: false),
                        Gender = c.Int(nullable: false),
                        IsOriginalTrainer = c.Boolean(nullable: false),
                        PokeballId = c.Int(nullable: false),
                        Nickname = c.String(unicode: false),
                        IsShiny = c.Boolean(nullable: false),
                        IsEvent = c.Boolean(nullable: false),
                        HasPokerus = c.Boolean(nullable: false),
                        CheatUsesd = c.Boolean(nullable: false),
                        RngUsed = c.Boolean(nullable: false),
                        IsClone = c.Boolean(nullable: false),
                        AbilityId = c.Int(nullable: false),
                        Attack1Id = c.Int(nullable: false),
                        Attack2Id = c.Int(nullable: false),
                        Attack3Id = c.Int(nullable: false),
                        Attack4Id = c.Int(nullable: false),
                        value_hp = c.Int(nullable: false),
                        value_attack = c.Int(nullable: false),
                        value_defense = c.Int(nullable: false),
                        value_special_attack = c.Int(nullable: false),
                        value_special_defense = c.Int(nullable: false),
                        value_speed = c.Int(nullable: false),
                        dv_hp = c.Int(nullable: false),
                        dv_attack = c.Int(nullable: false),
                        dv_defense = c.Int(nullable: false),
                        dv_special_attack = c.Int(nullable: false),
                        dv_special_defense = c.Int(nullable: false),
                        dv_speed = c.Int(nullable: false),
                        ev_hp = c.Int(nullable: false),
                        ev_attack = c.Int(nullable: false),
                        ev_defense = c.Int(nullable: false),
                        ev_special_attack = c.Int(nullable: false),
                        ev_special_defense = c.Int(nullable: false),
                        ev_speed = c.Int(nullable: false),
                        NatureId = c.Int(nullable: false),
                        Generation = c.Int(nullable: false),
                        Description = c.String(unicode: false),
                        DescriptionCode = c.String(unicode: false),
                        CreationDate = c.DateTime(nullable: false, precision: 0),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                        Status = c.Int(nullable: false),
                        ViewCount = c.Int(nullable: false),
                        AuctionEnd = c.DateTime(precision: 0),
                        CurrentPrice = c.Int(),
                        Reserve = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.dex_ability", t => t.AbilityId, cascadeDelete: true)
                .ForeignKey("dbo.Attacks", t => t.Attack1Id, cascadeDelete: true)
                .ForeignKey("dbo.Attacks", t => t.Attack2Id, cascadeDelete: true)
                .ForeignKey("dbo.Attacks", t => t.Attack3Id, cascadeDelete: true)
                .ForeignKey("dbo.Attacks", t => t.Attack4Id, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.ItemId)
                .ForeignKey("dbo.Natures", t => t.NatureId, cascadeDelete: true)
                .ForeignKey("dbo.Pokeballs", t => t.PokeballId, cascadeDelete: true)
                .ForeignKey("dbo.dex_pokemon", t => t.PokemonId)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ItemId)
                .Index(t => t.PokemonId)
                .Index(t => t.PokeballId)
                .Index(t => t.AbilityId)
                .Index(t => t.Attack1Id)
                .Index(t => t.Attack2Id)
                .Index(t => t.Attack3Id)
                .Index(t => t.Attack4Id)
                .Index(t => t.NatureId);
            
            CreateTable(
                "dbo.UserActivities",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false, precision: 0),
                        AppointmentId = c.Int(),
                        AttackId = c.Int(),
                        ContentId = c.Int(),
                        FeedbackId = c.Int(),
                        FeedbackId1 = c.Int(),
                        VersionId = c.Int(),
                        VersionNumber = c.Int(),
                        FanartId = c.Int(),
                        PokemonId = c.Int(),
                        MovesetId = c.Int(),
                        AttackId1 = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Appointments", t => t.AppointmentId, cascadeDelete: true)
                .ForeignKey("dbo.Attacks", t => t.AttackId, cascadeDelete: true)
                .ForeignKey("dbo.content", t => t.ContentId, cascadeDelete: true)
                .ForeignKey("dbo.content_feedback", t => t.FeedbackId)
                .ForeignKey("dbo.content_feedback", t => t.FeedbackId1)
                .ForeignKey("dbo.content_versions", t => t.VersionId, cascadeDelete: true)
                .ForeignKey("dbo.Fanarts", t => t.FanartId, cascadeDelete: true)
                .ForeignKey("dbo.dex_pokemon", t => t.PokemonId, cascadeDelete: true)
                .ForeignKey("dbo.Movesets", t => t.MovesetId, cascadeDelete: true)
                .ForeignKey("dbo.Attacks", t => t.AttackId1, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.AppointmentId)
                .Index(t => t.AttackId)
                .Index(t => t.ContentId)
                .Index(t => t.FeedbackId)
                .Index(t => t.FeedbackId1)
                .Index(t => t.VersionId)
                .Index(t => t.FanartId)
                .Index(t => t.PokemonId)
                .Index(t => t.MovesetId)
                .Index(t => t.AttackId1);
            
            CreateTable(
                "dbo.UserAdminTrackers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ModeratorId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Modul = c.String(maxLength: 20, storeType: "nvarchar"),
                        Timestamp = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_users", t => t.ModeratorId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ModeratorId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserEmployeeOfTheMonths",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Month = c.DateTime(nullable: false, precision: 0),
                        Reason = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserNotifications",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Sent = c.DateTime(nullable: false, precision: 0),
                        IsUnread = c.Boolean(nullable: false),
                        ReadTime = c.DateTime(nullable: false, precision: 0),
                        Title = c.String(unicode: false),
                        Message = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserPages",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Content = c.String(unicode: false),
                        ContentCode = c.String(unicode: false),
                        UsesFullWidth = c.Boolean(),
                        CanBeIndexed = c.Boolean(nullable: false),
                        Stylesheet = c.String(unicode: false),
                        StylesheetCode = c.String(unicode: false),
                        UsesTabs = c.Boolean(nullable: false),
                        RedirectUrl = c.String(unicode: false),
                        LastUpdate = c.DateTime(nullable: false, precision: 0),
                        CreationTime = c.DateTime(nullable: false, precision: 0),
                        ViewCount = c.Int(nullable: false),
                        LastViewIpAddress = c.String(maxLength: 39, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CkEditorShowSource = c.Boolean(nullable: false),
                        JabberId = c.String(maxLength: 100, storeType: "nvarchar"),
                        IrcOnlineTime = c.String(maxLength: 100, storeType: "nvarchar"),
                        GooglePlusUrl = c.String(maxLength: 100, storeType: "nvarchar"),
                        Age = c.Int(nullable: false),
                        Hometown = c.String(maxLength: 100, storeType: "nvarchar"),
                        Gender = c.String(maxLength: 100, storeType: "nvarchar"),
                        Interests = c.String(maxLength: 255, storeType: "nvarchar"),
                        FoundedIrcChannels = c.String(maxLength: 100, storeType: "nvarchar"),
                        FavoritePokemon = c.String(maxLength: 100, storeType: "nvarchar"),
                        Homestate = c.String(maxLength: 100, storeType: "nvarchar"),
                        IcqNumber = c.String(maxLength: 100, storeType: "nvarchar"),
                        PokemonExperteName = c.String(maxLength: 100, storeType: "nvarchar"),
                        FirstName = c.String(maxLength: 100, storeType: "nvarchar"),
                        Occupation = c.String(maxLength: 100, storeType: "nvarchar"),
                        PokemonShowdownName = c.String(maxLength: 100, storeType: "nvarchar"),
                        BisafansName = c.String(maxLength: 100, storeType: "nvarchar"),
                        FilbBoardName = c.String(maxLength: 100, storeType: "nvarchar"),
                        IrcNickname = c.String(maxLength: 100, storeType: "nvarchar"),
                        IrcAlternativeNickname = c.String(maxLength: 100, storeType: "nvarchar"),
                        IrcChannels = c.String(maxLength: 255, storeType: "nvarchar"),
                        SmogonName = c.String(maxLength: 100, storeType: "nvarchar"),
                        IrcHostmask = c.String(maxLength: 100, storeType: "nvarchar"),
                        SerebiiName = c.String(maxLength: 100, storeType: "nvarchar"),
                        OldUsername = c.String(maxLength: 100, storeType: "nvarchar"),
                        TwitterUrl = c.String(maxLength: 100, storeType: "nvarchar"),
                        FacebookUrl = c.String(maxLength: 100, storeType: "nvarchar"),
                        DeviantArtUrl = c.String(maxLength: 100, storeType: "nvarchar"),
                        YoutubeUrl = c.String(maxLength: 100, storeType: "nvarchar"),
                        SkypeUrl = c.String(maxLength: 100, storeType: "nvarchar"),
                        SoupIoUrl = c.String(maxLength: 100, storeType: "nvarchar"),
                        LastFmName = c.String(maxLength: 100, storeType: "nvarchar"),
                        PinterestUrl = c.String(maxLength: 100, storeType: "nvarchar"),
                        FriendfeedName = c.String(maxLength: 100, storeType: "nvarchar"),
                        DropIoName = c.String(maxLength: 100, storeType: "nvarchar"),
                        WerKenntWenUrl = c.String(maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserUploads",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        Status = c.Int(nullable: false),
                        IsLocked = c.Boolean(nullable: false),
                        IsCopyrightViolation = c.Boolean(nullable: false),
                        Url = c.String(maxLength: 255, storeType: "nvarchar"),
                        Hash = c.String(maxLength: 68, storeType: "nvarchar"),
                        OriginalFileName = c.String(maxLength: 100, storeType: "nvarchar"),
                        IsImage = c.Boolean(nullable: false),
                        FileExtension = c.String(maxLength: 10, storeType: "nvarchar"),
                        MimeType = c.String(maxLength: 255, storeType: "nvarchar"),
                        FileSize = c.Int(nullable: false),
                        ImageWidth = c.Int(nullable: false),
                        ImageHeight = c.Int(nullable: false),
                        UploadIp = c.String(maxLength: 39, storeType: "nvarchar"),
                        UploadTime = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Interests",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false, precision: 0),
                        Ip = c.String(maxLength: 39, storeType: "nvarchar"),
                        OfferId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Offers", t => t.OfferId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.OfferId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Interests", "UserId", "dbo.system_users");
            DropForeignKey("dbo.Interests", "OfferId", "dbo.Offers");
            DropForeignKey("dbo.UserUploads", "UserId", "dbo.system_users");
            DropForeignKey("dbo.UserProfiles", "UserId", "dbo.system_users");
            DropForeignKey("dbo.UserPages", "UserId", "dbo.system_users");
            DropForeignKey("dbo.UserNotifications", "UserId", "dbo.system_users");
            DropForeignKey("dbo.UserEmployeeOfTheMonths", "UserId", "dbo.system_users");
            DropForeignKey("dbo.UserAdminTrackers", "UserId", "dbo.system_users");
            DropForeignKey("dbo.UserAdminTrackers", "ModeratorId", "dbo.system_users");
            DropForeignKey("dbo.UserActivities", "AttackId1", "dbo.Attacks");
            DropForeignKey("dbo.UserActivities", "MovesetId", "dbo.Movesets");
            DropForeignKey("dbo.UserActivities", "PokemonId", "dbo.dex_pokemon");
            DropForeignKey("dbo.UserActivities", "FanartId", "dbo.Fanarts");
            DropForeignKey("dbo.UserActivities", "VersionId", "dbo.content_versions");
            DropForeignKey("dbo.UserActivities", "FeedbackId1", "dbo.content_feedback");
            DropForeignKey("dbo.UserActivities", "FeedbackId", "dbo.content_feedback");
            DropForeignKey("dbo.UserActivities", "ContentId", "dbo.content");
            DropForeignKey("dbo.UserActivities", "AttackId", "dbo.Attacks");
            DropForeignKey("dbo.UserActivities", "AppointmentId", "dbo.Appointments");
            DropForeignKey("dbo.UserActivities", "UserId", "dbo.system_users");
            DropForeignKey("dbo.TradeLogs", "PokemonId", "dbo.dex_pokemon");
            DropForeignKey("dbo.TradeLogs", "OfferId", "dbo.Offers");
            DropForeignKey("dbo.Offers", "UserId", "dbo.system_users");
            DropForeignKey("dbo.Offers", "PokemonId", "dbo.dex_pokemon");
            DropForeignKey("dbo.Offers", "PokeballId", "dbo.Pokeballs");
            DropForeignKey("dbo.Offers", "NatureId", "dbo.Natures");
            DropForeignKey("dbo.Offers", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Offers", "Attack4Id", "dbo.Attacks");
            DropForeignKey("dbo.Offers", "Attack3Id", "dbo.Attacks");
            DropForeignKey("dbo.Offers", "Attack2Id", "dbo.Attacks");
            DropForeignKey("dbo.Offers", "Attack1Id", "dbo.Attacks");
            DropForeignKey("dbo.Offers", "AbilityId", "dbo.dex_ability");
            DropForeignKey("dbo.TournamentParticipations", "UserId", "dbo.system_users");
            DropForeignKey("dbo.TournamentParticipations", "TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.Tournaments", "MetagameVersusId", "dbo.MetagameVersus");
            DropForeignKey("dbo.Tournaments", "TierId", "dbo.dex_tiers");
            DropForeignKey("dbo.Tournaments", "MetagameId", "dbo.Metagames");
            DropForeignKey("dbo.Tournaments", "TournamentClassId", "dbo.TournamentClasses");
            DropForeignKey("dbo.SearchTrackers", "UserId", "dbo.system_users");
            DropForeignKey("dbo.Reviews", "UserId", "dbo.system_users");
            DropForeignKey("dbo.Reviews", "ReviewItemId", "dbo.ReviewItems");
            DropForeignKey("dbo.RefLinkTrackers", "UserId", "dbo.system_users");
            DropForeignKey("dbo.RangerPokemons", "PokePowerId", "dbo.PokePowers");
            DropForeignKey("dbo.RangerPokemons", "PokemonId", "dbo.dex_pokemon");
            DropForeignKey("dbo.RangerPokemons", "TypeId", "dbo.dex_types");
            DropForeignKey("dbo.RangerPokemons", "RangerAbilityId", "dbo.RangerAbilities");
            DropForeignKey("dbo.QrCodes", "UserId", "dbo.system_users");
            DropForeignKey("dbo.QrCodes", "LocationId", "dbo.PokemonLocations");
            DropForeignKey("dbo.QrCodes", "FriendCodeId", "dbo.Friendcodes");
            DropForeignKey("dbo.PokemonTypeEffectivities", "Type2Id", "dbo.dex_types");
            DropForeignKey("dbo.PokemonTypeEffectivities", "Type1Id", "dbo.dex_types");
            DropForeignKey("dbo.dex_pokemon_tiers", "TierId", "dbo.dex_tiers");
            DropForeignKey("dbo.dex_pokemon_tiers", "PokemonId", "dbo.dex_pokemon");
            DropForeignKey("dbo.PokemonPokedexDescriptions", "PokemonId", "dbo.dex_pokemon");
            DropForeignKey("dbo.PokemonPokedexDescriptions", "EditionId", "dbo.PokemonEditions");
            DropForeignKey("dbo.PokemonLocations", "UpdateUserId", "dbo.system_users");
            DropForeignKey("dbo.PokemonLocations", "PokemonId", "dbo.dex_pokemon");
            DropForeignKey("dbo.PokemonLocations", "LocationMethodId", "dbo.PokemonLocationMethods");
            DropForeignKey("dbo.PokemonLocations", "EditionId", "dbo.PokemonEditions");
            DropForeignKey("dbo.PokemonItems", "PokemonId", "dbo.dex_pokemon");
            DropForeignKey("dbo.PokemonItems", "ItemId", "dbo.Items");
            DropForeignKey("dbo.PokemonItems", "EditionGroupId", "dbo.PokemonEditionGroups");
            DropForeignKey("dbo.PokemonItems", "EditionId", "dbo.PokemonEditions");
            DropForeignKey("dbo.PokemonImages", "UploadUserId", "dbo.system_users");
            DropForeignKey("dbo.PokemonImages", "PokemonId", "dbo.dex_pokemon");
            DropForeignKey("dbo.PokemonAttacks", "AttackTechnicalMachineId", "dbo.AttackTechnicalMachines");
            DropForeignKey("dbo.PokemonAttacks", "PokemonId", "dbo.dex_pokemon");
            DropForeignKey("dbo.PokemonAttacks", "EditionId", "dbo.PokemonEditions");
            DropForeignKey("dbo.PokemonAttacks", "AttackId", "dbo.Attacks");
            DropForeignKey("dbo.Movesets", "UpdateUserId", "dbo.system_users");
            DropForeignKey("dbo.Movesets", "PokemonId", "dbo.dex_pokemon");
            DropForeignKey("dbo.Movesets", "AuthorUserId", "dbo.system_users");
            DropForeignKey("dbo.Movesets", "ApprovalUserId", "dbo.system_users");
            DropForeignKey("dbo.ItemLocations", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.ItemLocations", "PokemonEditionGroupId", "dbo.PokemonEditionGroups");
            DropForeignKey("dbo.ItemLocations", "EditionId", "dbo.PokemonEditions");
            DropForeignKey("dbo.PokemonEditions", "PokemonEditionGroupId", "dbo.PokemonEditionGroups");
            DropForeignKey("dbo.FriendSafaris", "UserId", "dbo.system_users");
            DropForeignKey("dbo.FriendSafaris", "PokemonTypeId", "dbo.dex_types");
            DropForeignKey("dbo.FriendSafaris", "Pokemon3Id", "dbo.dex_pokemon");
            DropForeignKey("dbo.FriendSafaris", "Pokemon2Id", "dbo.dex_pokemon");
            DropForeignKey("dbo.FriendSafaris", "Pokemon1Id", "dbo.dex_pokemon");
            DropForeignKey("dbo.FriendSafaris", "FriendCodeId", "dbo.Friendcodes");
            DropForeignKey("dbo.Friendcodes", "UserId", "dbo.system_users");
            DropForeignKey("dbo.Friendcodes", "GameId", "dbo.FriendcodeGames");
            DropForeignKey("dbo.FanartTags", "TagId", "dbo.FanartTags1");
            DropForeignKey("dbo.FanartTags", "FanartId", "dbo.Fanarts");
            DropForeignKey("dbo.FanartFavorites", "UserId", "dbo.system_users");
            DropForeignKey("dbo.FanartFavorites", "FanartId", "dbo.Fanarts");
            DropForeignKey("dbo.Fanarts", "PokemonId", "dbo.dex_pokemon");
            DropForeignKey("dbo.Fanarts", "CategoryId", "dbo.FanartCategories");
            DropForeignKey("dbo.Counters", "PokemonId", "dbo.dex_pokemon");
            DropForeignKey("dbo.Counters", "CounterId", "dbo.dex_pokemon");
            DropForeignKey("dbo.dex_strategy", "PokemonId", "dbo.dex_pokemon");
            DropForeignKey("dbo.PokemonAbilities", "PokemonId", "dbo.dex_pokemon");
            DropForeignKey("dbo.PokemonAbilities", "AbilityId", "dbo.dex_ability");
            DropForeignKey("dbo.dex_pokemon", "FootprintId", "dbo.dex_footprints");
            DropForeignKey("dbo.dex_pokemon", "EvolutionParentId", "dbo.dex_pokemon");
            DropForeignKey("dbo.dex_pokemon", "EvolutionItemId", "dbo.Items");
            DropForeignKey("dbo.dex_pokemon", "EvolutionBaseId", "dbo.dex_pokemon");
            DropForeignKey("dbo.dex_pokemon", "ColorId", "dbo.dex_color");
            DropForeignKey("dbo.dex_pokemon", "BreedingGroup2Id", "dbo.dex_breedinggroups");
            DropForeignKey("dbo.dex_pokemon", "BreedingGroup1Id", "dbo.dex_breedinggroups");
            DropForeignKey("dbo.dex_pokemon", "BodyFormId", "dbo.dex_bodyforms");
            DropForeignKey("dbo.ChatAppletTrackers", "UserId", "dbo.system_users");
            DropForeignKey("dbo.Berries", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Berries", "AttackTypeId", "dbo.dex_types");
            DropForeignKey("dbo.AttackHiddenMachines", "AttackHiddenMachineId", "dbo.AttackHiddenMachines");
            DropForeignKey("dbo.AttackHiddenMachines", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Attacks", "UpdateUserId", "dbo.system_users");
            DropForeignKey("dbo.Attacks", "TypeId", "dbo.dex_types");
            DropForeignKey("dbo.Attacks", "ContestTypeId", "dbo.PokemonContestTypes");
            DropForeignKey("dbo.AttackTechnicalMachines", "AttackId", "dbo.Attacks");
            DropForeignKey("dbo.AttackTechnicalMachines", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Items", "UpdateUserId", "dbo.system_users");
            DropForeignKey("dbo.AttackTargets", "AttackId", "dbo.Attacks");
            DropForeignKey("dbo.AttackTargets", "AttackTargetTypeId", "dbo.AttackTargetTypes");
            DropForeignKey("dbo.AttackHiddenMachines", "AttackId", "dbo.Attacks");
            DropForeignKey("dbo.AppointmentSentNotifications", "NotificationId", "dbo.AppointmentNotifications");
            DropForeignKey("dbo.AppointmentSentNotifications", "AppointmentId", "dbo.Appointments");
            DropForeignKey("dbo.AppointmentParticipations", "UserId", "dbo.system_users");
            DropForeignKey("dbo.AppointmentParticipations", "AppointmentId", "dbo.Appointments");
            DropForeignKey("dbo.AppointmentNotifications", "UserId", "dbo.system_users");
            DropForeignKey("dbo.AppointmentNotifications", "EventTypeId", "dbo.AppointmentTypes");
            DropForeignKey("dbo.AppointmentNotifications", "AppointmentId", "dbo.Appointments");
            DropForeignKey("dbo.Appointments", "EditorId", "dbo.system_users");
            DropForeignKey("dbo.Appointments", "AuthorId", "dbo.system_users");
            DropForeignKey("dbo.Appointments", "AppointmentTypeId", "dbo.AppointmentTypes");
            DropForeignKey("dbo.dex_ability", "LastUpdateUserId", "dbo.system_users");
            DropIndex("dbo.Interests", new[] { "OfferId" });
            DropIndex("dbo.Interests", new[] { "UserId" });
            DropIndex("dbo.UserUploads", new[] { "UserId" });
            DropIndex("dbo.UserProfiles", new[] { "UserId" });
            DropIndex("dbo.UserPages", new[] { "UserId" });
            DropIndex("dbo.UserNotifications", new[] { "UserId" });
            DropIndex("dbo.UserEmployeeOfTheMonths", new[] { "UserId" });
            DropIndex("dbo.UserAdminTrackers", new[] { "UserId" });
            DropIndex("dbo.UserAdminTrackers", new[] { "ModeratorId" });
            DropIndex("dbo.UserActivities", new[] { "AttackId1" });
            DropIndex("dbo.UserActivities", new[] { "MovesetId" });
            DropIndex("dbo.UserActivities", new[] { "PokemonId" });
            DropIndex("dbo.UserActivities", new[] { "FanartId" });
            DropIndex("dbo.UserActivities", new[] { "VersionId" });
            DropIndex("dbo.UserActivities", new[] { "FeedbackId1" });
            DropIndex("dbo.UserActivities", new[] { "FeedbackId" });
            DropIndex("dbo.UserActivities", new[] { "ContentId" });
            DropIndex("dbo.UserActivities", new[] { "AttackId" });
            DropIndex("dbo.UserActivities", new[] { "AppointmentId" });
            DropIndex("dbo.UserActivities", new[] { "UserId" });
            DropIndex("dbo.Offers", new[] { "NatureId" });
            DropIndex("dbo.Offers", new[] { "Attack4Id" });
            DropIndex("dbo.Offers", new[] { "Attack3Id" });
            DropIndex("dbo.Offers", new[] { "Attack2Id" });
            DropIndex("dbo.Offers", new[] { "Attack1Id" });
            DropIndex("dbo.Offers", new[] { "AbilityId" });
            DropIndex("dbo.Offers", new[] { "PokeballId" });
            DropIndex("dbo.Offers", new[] { "PokemonId" });
            DropIndex("dbo.Offers", new[] { "ItemId" });
            DropIndex("dbo.Offers", new[] { "UserId" });
            DropIndex("dbo.TradeLogs", new[] { "OfferId" });
            DropIndex("dbo.TradeLogs", new[] { "PokemonId" });
            DropIndex("dbo.TournamentParticipations", new[] { "TournamentId" });
            DropIndex("dbo.TournamentParticipations", new[] { "UserId" });
            DropIndex("dbo.Tournaments", new[] { "TournamentClassId" });
            DropIndex("dbo.Tournaments", new[] { "MetagameVersusId" });
            DropIndex("dbo.Tournaments", new[] { "TierId" });
            DropIndex("dbo.Tournaments", new[] { "MetagameId" });
            DropIndex("dbo.SearchTrackers", new[] { "UserId" });
            DropIndex("dbo.Reviews", new[] { "ReviewItemId" });
            DropIndex("dbo.Reviews", new[] { "UserId" });
            DropIndex("dbo.RefLinkTrackers", new[] { "UserId" });
            DropIndex("dbo.RangerPokemons", new[] { "RangerAbilityId" });
            DropIndex("dbo.RangerPokemons", new[] { "PokePowerId" });
            DropIndex("dbo.RangerPokemons", new[] { "TypeId" });
            DropIndex("dbo.RangerPokemons", new[] { "PokemonId" });
            DropIndex("dbo.QrCodes", new[] { "LocationId" });
            DropIndex("dbo.QrCodes", new[] { "FriendCodeId" });
            DropIndex("dbo.QrCodes", new[] { "UserId" });
            DropIndex("dbo.PokemonTypeEffectivities", new[] { "Type2Id" });
            DropIndex("dbo.PokemonTypeEffectivities", new[] { "Type1Id" });
            DropIndex("dbo.dex_pokemon_tiers", new[] { "TierId" });
            DropIndex("dbo.dex_pokemon_tiers", new[] { "PokemonId" });
            DropIndex("dbo.PokemonPokedexDescriptions", new[] { "PokemonId" });
            DropIndex("dbo.PokemonPokedexDescriptions", new[] { "EditionId" });
            DropIndex("dbo.PokemonLocations", new[] { "UpdateUserId" });
            DropIndex("dbo.PokemonLocations", new[] { "EditionId" });
            DropIndex("dbo.PokemonLocations", new[] { "LocationMethodId" });
            DropIndex("dbo.PokemonLocations", new[] { "PokemonId" });
            DropIndex("dbo.PokemonItems", new[] { "ItemId" });
            DropIndex("dbo.PokemonItems", new[] { "EditionGroupId" });
            DropIndex("dbo.PokemonItems", new[] { "EditionId" });
            DropIndex("dbo.PokemonItems", new[] { "PokemonId" });
            DropIndex("dbo.PokemonImages", new[] { "UploadUserId" });
            DropIndex("dbo.PokemonImages", new[] { "PokemonId" });
            DropIndex("dbo.PokemonAttacks", new[] { "AttackTechnicalMachineId" });
            DropIndex("dbo.PokemonAttacks", new[] { "AttackId" });
            DropIndex("dbo.PokemonAttacks", new[] { "PokemonId" });
            DropIndex("dbo.PokemonAttacks", new[] { "EditionId" });
            DropIndex("dbo.Movesets", new[] { "UpdateUserId" });
            DropIndex("dbo.Movesets", new[] { "AuthorUserId" });
            DropIndex("dbo.Movesets", new[] { "ApprovalUserId" });
            DropIndex("dbo.Movesets", new[] { "PokemonId" });
            DropIndex("dbo.PokemonEditions", new[] { "PokemonEditionGroupId" });
            DropIndex("dbo.ItemLocations", new[] { "PokemonEditionGroupId" });
            DropIndex("dbo.ItemLocations", new[] { "EditionId" });
            DropIndex("dbo.ItemLocations", new[] { "LocationId" });
            DropIndex("dbo.FriendSafaris", new[] { "Pokemon3Id" });
            DropIndex("dbo.FriendSafaris", new[] { "Pokemon2Id" });
            DropIndex("dbo.FriendSafaris", new[] { "Pokemon1Id" });
            DropIndex("dbo.FriendSafaris", new[] { "PokemonTypeId" });
            DropIndex("dbo.FriendSafaris", new[] { "FriendCodeId" });
            DropIndex("dbo.FriendSafaris", new[] { "UserId" });
            DropIndex("dbo.Friendcodes", new[] { "GameId" });
            DropIndex("dbo.Friendcodes", new[] { "UserId" });
            DropIndex("dbo.FanartTags", new[] { "TagId" });
            DropIndex("dbo.FanartTags", new[] { "FanartId" });
            DropIndex("dbo.Fanarts", new[] { "PokemonId" });
            DropIndex("dbo.Fanarts", new[] { "CategoryId" });
            DropIndex("dbo.FanartFavorites", new[] { "UserId" });
            DropIndex("dbo.FanartFavorites", new[] { "FanartId" });
            DropIndex("dbo.dex_strategy", new[] { "PokemonId" });
            DropIndex("dbo.PokemonAbilities", new[] { "AbilityId" });
            DropIndex("dbo.PokemonAbilities", new[] { "PokemonId" });
            DropIndex("dbo.dex_pokemon", new[] { "BodyFormId" });
            DropIndex("dbo.dex_pokemon", new[] { "FootprintId" });
            DropIndex("dbo.dex_pokemon", new[] { "ColorId" });
            DropIndex("dbo.dex_pokemon", new[] { "BreedingGroup2Id" });
            DropIndex("dbo.dex_pokemon", new[] { "BreedingGroup1Id" });
            DropIndex("dbo.dex_pokemon", new[] { "EvolutionParentId" });
            DropIndex("dbo.dex_pokemon", new[] { "EvolutionItemId" });
            DropIndex("dbo.dex_pokemon", new[] { "EvolutionBaseId" });
            DropIndex("dbo.dex_pokemon", new[] { "url" });
            DropIndex("dbo.Counters", new[] { "CounterId" });
            DropIndex("dbo.Counters", new[] { "PokemonId" });
            DropIndex("dbo.ChatAppletTrackers", new[] { "UserId" });
            DropIndex("dbo.Berries", new[] { "ItemId" });
            DropIndex("dbo.Berries", new[] { "AttackTypeId" });
            DropIndex("dbo.Items", new[] { "UpdateUserId" });
            DropIndex("dbo.AttackTechnicalMachines", new[] { "ItemId" });
            DropIndex("dbo.AttackTechnicalMachines", new[] { "AttackId" });
            DropIndex("dbo.AttackTargets", new[] { "AttackTargetTypeId" });
            DropIndex("dbo.AttackTargets", new[] { "AttackId" });
            DropIndex("dbo.Attacks", new[] { "ContestTypeId" });
            DropIndex("dbo.Attacks", new[] { "UpdateUserId" });
            DropIndex("dbo.Attacks", new[] { "TypeId" });
            DropIndex("dbo.AttackHiddenMachines", new[] { "AttackHiddenMachineId" });
            DropIndex("dbo.AttackHiddenMachines", new[] { "ItemId" });
            DropIndex("dbo.AttackHiddenMachines", new[] { "AttackId" });
            DropIndex("dbo.AppointmentSentNotifications", new[] { "NotificationId" });
            DropIndex("dbo.AppointmentSentNotifications", new[] { "AppointmentId" });
            DropIndex("dbo.AppointmentParticipations", new[] { "UserId" });
            DropIndex("dbo.AppointmentParticipations", new[] { "AppointmentId" });
            DropIndex("dbo.Appointments", new[] { "EditorId" });
            DropIndex("dbo.Appointments", new[] { "AuthorId" });
            DropIndex("dbo.Appointments", new[] { "IsHidden" });
            DropIndex("dbo.Appointments", new[] { "Url" });
            DropIndex("dbo.Appointments", new[] { "DisplayInForum" });
            DropIndex("dbo.Appointments", new[] { "AppointmentTypeId" });
            DropIndex("dbo.Appointments", new[] { "End" });
            DropIndex("dbo.Appointments", new[] { "Begin" });
            DropIndex("dbo.AppointmentNotifications", new[] { "AppointmentId" });
            DropIndex("dbo.AppointmentNotifications", new[] { "UserId" });
            DropIndex("dbo.AppointmentNotifications", new[] { "EventTypeId" });
            DropIndex("dbo.dex_ability", new[] { "LastUpdateUserId" });
            DropTable("dbo.Interests");
            DropTable("dbo.UserUploads");
            DropTable("dbo.UserProfiles");
            DropTable("dbo.UserPages");
            DropTable("dbo.UserNotifications");
            DropTable("dbo.UserEmployeeOfTheMonths");
            DropTable("dbo.UserAdminTrackers");
            DropTable("dbo.UserActivities");
            DropTable("dbo.Offers");
            DropTable("dbo.TradeLogs");
            DropTable("dbo.TournamentParticipations");
            DropTable("dbo.TournamentClasses");
            DropTable("dbo.Tournaments");
            DropTable("dbo.SearchTrackers");
            DropTable("dbo.Reviews");
            DropTable("dbo.ReviewItems");
            DropTable("dbo.ReviewIndexes");
            DropTable("dbo.RefLinkTrackers");
            DropTable("dbo.RangerPokemonLocations");
            DropTable("dbo.RangerPokemons");
            DropTable("dbo.RangerLocations");
            DropTable("dbo.RangerAbilities");
            DropTable("dbo.QrCodes");
            DropTable("dbo.PokePowers");
            DropTable("dbo.PokemonTypeEffectivities");
            DropTable("dbo.dex_pokemon_tiers");
            DropTable("dbo.dex_tiers");
            DropTable("dbo.PokemonPokedexDescriptions");
            DropTable("dbo.PokemonLocations");
            DropTable("dbo.PokemonLocationMethods");
            DropTable("dbo.PokemonItems");
            DropTable("dbo.PokemonImages");
            DropTable("dbo.PokemonAttacks");
            DropTable("dbo.Pokeballs");
            DropTable("dbo.Natures");
            DropTable("dbo.MysteryDungeonReaches");
            DropTable("dbo.Movesets");
            DropTable("dbo.MetagameVersus");
            DropTable("dbo.Metagames");
            DropTable("dbo.LexiconEntries");
            DropTable("dbo.Locations");
            DropTable("dbo.PokemonEditionGroups");
            DropTable("dbo.PokemonEditions");
            DropTable("dbo.ItemLocations");
            DropTable("dbo.FriendSafaris");
            DropTable("dbo.Friendcodes");
            DropTable("dbo.FriendcodeGames");
            DropTable("dbo.FanartTags1");
            DropTable("dbo.FanartTags");
            DropTable("dbo.Fanarts");
            DropTable("dbo.FanartFavorites");
            DropTable("dbo.FanartCategories");
            DropTable("dbo.dex_strategy");
            DropTable("dbo.PokemonAbilities");
            DropTable("dbo.dex_footprints");
            DropTable("dbo.dex_color");
            DropTable("dbo.dex_breedinggroups");
            DropTable("dbo.dex_bodyforms");
            DropTable("dbo.dex_pokemon");
            DropTable("dbo.Counters");
            DropTable("dbo.ChatAppletTrackers");
            DropTable("dbo.Berries");
            DropTable("dbo.dex_types");
            DropTable("dbo.PokemonContestTypes");
            DropTable("dbo.Items");
            DropTable("dbo.AttackTechnicalMachines");
            DropTable("dbo.AttackTargetTypes");
            DropTable("dbo.AttackTargets");
            DropTable("dbo.Attacks");
            DropTable("dbo.AttackHiddenMachines");
            DropTable("dbo.AppointmentSentNotifications");
            DropTable("dbo.AppointmentParticipations");
            DropTable("dbo.AppointmentTypes");
            DropTable("dbo.Appointments");
            DropTable("dbo.AppointmentNotifications");
            DropTable("dbo.AppointmentAreas");
            DropTable("dbo.dex_ability");
        }
    }
}
