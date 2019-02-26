// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data.Calendar
{
    public class AppointmentSentNotification
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int AppointmentId { get; set; }

        [ForeignKey("AppointmentId")]
        public Appointment Appointment { get; set; }

        public DateTime NotificationTime { get; set; }

        public int NotificationId { get; set; }

        [ForeignKey("NotificationId")]
        public AppointmentNotification Notification { get; set; }
    }
}

