using System;
using Pokefans.Data.Calendar;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data.UserData.Activities
{
    public class AppointmentActivity : UserActivity
    {
        public int AppointmentId { get; set; }

        [ForeignKey("AppointmentId")]
        public Appointment Appointment { get; set; }
    }
}

