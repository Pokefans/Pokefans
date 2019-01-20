using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data
{
    public class DsgvoComplianceInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime EffectiveTime { get; set; }

        /// <summary>
        /// the current data protection statement, rendered as HTML.
        /// </summary>
        /// <value>The data protection statement.</value>
        public string DataProtectionStatement { get; set; }

        public string TermsOfUsage { get; set; }

        public string ForumRules { get; set; }
    }
}