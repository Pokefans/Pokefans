// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Pokefans.Data.Pokedex;

namespace Pokefans.Data.Strategy
{
    public enum TournamentStatus
    {
        Aborted = -1,
        Running = 0,
        Finished = 1}

    ;

    public class Tournament
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int MetagameId  { get; set; }

        [ForeignKey("MetagameId")]
        public Metagame Metagame { get; set; }

        public int TierId { get; set; }

        [ForeignKey("TierId")]
        public PokemonTier Tier { get; set; }

        public int MetagameVersusId { get; set; }

        [ForeignKey("MetagameVersusId")]
        public MetagameVersus Versus { get; set; }

        public int TournamentClassId { get; set; }

        [ForeignKey("TournamentClassId")]
        public TournamentClass Class { get; set; }

        public DateTime CreationDate { get; set; }
    }
}

