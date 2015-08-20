using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data.UserData.Activities
{
    public class ContentActivity : UserActivity
    {
        public int ContentId { get; set; }

        [ForeignKey("ContentId")]
        public int Content { get; set; }
    }
}

