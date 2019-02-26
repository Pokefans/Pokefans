// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.Tracker
{
    public class PokedexSearchTracker
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Input { get; set; }

        [MaxLength(10)]
        public string Index { get; set; }

        [MaxLength(255)]
        public string Referer { get; set; }

        // This is one of the few examples I can think of where violating any basic
        // normalization principle is kind of ok. Still, I'd like to have a nice
        // view of it in code.

        public string RawResult { get; set; }

        [NotMapped]
        public List<int> Result
        {
            get
            {
                return RawResult.Split(',').Select(g => int.Parse(g)).ToList();
            }
            set
            { 
                RawResult = String.Join(",", value.Select(g => g.ToString()));
            }
        }

        public int ResultCount { get; set; }

        public bool IsExactResult { get; set; }
    }
}

