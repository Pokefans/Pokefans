using System;
using System.Collections.Generic;
using Pokefans.Data;
using Pokefans.Data.UserData;


namespace Pokefans.Areas.user.Models
{
    public class PrivateMessageViewModel
    {
        public PrivateMessage Message { get; set; }
        public User From { get; set; }

		public int DeleteKey { get; set; }
		public bool IsInbox { get; set; }

        public List<PrivateMessageLabel> MessageLabels { get; set; }
        public Dictionary<int, PrivateMessageLabel> Labels { get; set; }

        public DateTime? ReportTime { get; set; }
    }
}

