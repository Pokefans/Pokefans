using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data.UserData.Activities
{
    public class ContentUpdateActivity : ContentActivity
    {
        public int VersionId { get; set; }

        [ForeignKey("VersionId")]
        public ContentVersion Version { get; set; }

        public int VersionNumber { get; set; }
    }
}

