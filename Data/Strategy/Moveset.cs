// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Pokefans.Data.Pokedex;

namespace Pokefans.Data.Strategy
{
    public enum MovesetClassification
    {
        Okay,
        Alternative,
        Recommendable,
        Standard}

    ;

    public enum MovesetStatus
    {
        Rejected = -1,
        Undecided = 0,
        Accepted = 1,
    };

    public class Moveset
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int PokemonId { get; set; }

        [ForeignKey("PokemonId")]
        public Pokemon Pokemon { get; set; }

        public PokemonGeneration Generation { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        public string Set { get; set; }

        public string SetCode { get; set; }

        public string Description { get; set; }

        public string DescriptionCode { get; set; }

        public MovesetClassification? Classification { get; set; }

        public MovesetStatus Status { get; set; }

        public string RejectReason { get; set; }

        public int ApprovalUserId { get; set; }

        [ForeignKey("ApprovalUserId")]
        public User ApprovalUser { get; set; }

        public DateTime ApprovalTime { get; set; }

        public int? AuthorUserId { get; set; }

        [ForeignKey("AuthorUserId")]
        public User Author { get; set; }

        [MaxLength(46)]
        public string AuthorIp { get; set; }

        public DateTime SubmissionTime { get; set; }

        public string AuthorUserAgent { get; set; }

        public int AuthorTiming { get; set; }

        public int UpdateUserId { get; set; }

        [ForeignKey("UpdateUserId")]
        public User UpdateUser { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}