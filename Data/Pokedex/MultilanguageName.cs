// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pokefans.Data.Pokedex
{
    public class MultilanguageName
    {
        /// <summary>
        /// Gets or sets the german name.
        /// </summary>
        /// <value>
        /// The german name.
        /// </value>
        public string German { get; set; }

        /// <summary>
        /// Gets or sets the english name.
        /// </summary>
        /// <value>
        /// The english name.
        /// </value>
        public string English { get; set; }

        /// <summary>
        /// Gets or sets the japanese name.
        /// </summary>
        /// <value>
        /// The japanese name.
        /// </value>
        public string Japanese { get; set; }

        /// <summary>
        /// Gets or sets the korean name.
        /// </summary>
        /// <value>
        /// The korean name.
        /// </value>
        public string Korean { get; set; }

        /// <summary>
        /// Gets or sets the french name.
        /// </summary>
        /// <value>
        /// The french name.
        /// </value>
        public string French { get; set; }

        /// <summary>
        /// Gets or sets the chinese name.
        /// </summary>
        /// <value>
        /// The chinese name.
        /// </value>
        public string Chinese { get; set; }

        /// <summary>
        /// Gets or sets the japnaese transcribed name.
        /// </summary>
        /// <value>
        /// The japnaese transcribed name.
        /// </value>
        public string JapnaeseTranscribed { get; set; }

        /// <summary>
        /// Gets or sets the korean transcribed name.
        /// </summary>
        /// <value>
        /// The korean transcribed name.
        /// </value>
        public string KoreanTranscribed { get; set; }

        /// <summary>
        /// Gets or sets the chinese transcribed name.
        /// </summary>
        /// <value>
        /// The chinese transcribed name.
        /// </value>
        public string ChineseTranscribed { get; set; }

        public override string ToString()
        {
            return German;
        }
    }
}
