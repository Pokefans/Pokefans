// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data.Calendar
{
    public class Appointment
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Key]
        public DateTime Begin { get; set; }

        [Key]
        public DateTime End { get; set; }

        public int AppointmentTypeId { get; set; }

        [ForeignKey("AppointmentTypeId")]
        public AppointmentType AppointmentType { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }

        public string DescriptionCode { get; set; }

        [MaxLength(250)]
        public string Teaser { get; set; }

        [Key]
        public bool DisplayInForum { get; set; }

        [MaxLength(100)]
        [Key]
        public string Url { get; set; }

        public bool CanParticipate { get; set; }

        [Key]
        public bool IsHidden { get; set; }

        public int AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public User Author { get; set; }

        public int EditorId { get; set; }

        [ForeignKey("EditorId")]
        public User Editor { get; set; }

        public DateTime LastEditTime { get; set; }

        public DateTime CreationTime { get; set; }
    }
}