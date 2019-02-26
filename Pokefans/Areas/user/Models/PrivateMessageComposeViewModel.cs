using System;
using System.Collections.Generic;
using Pokefans.Data;
using Pokefans.Data.UserData;


namespace Pokefans.Areas.user.Models
{
    public class PrivateMessageComposeViewModel
    {
        public string To { get; set; }
        public string Bcc { get; set; }

        public Guid? ConversationId { get; set; }

        public int? ReplyTo { get; set; }

        public string Body { get; set; }

        public string Subject { get; set; }
        
		public List<OldMessageViewModel> OldMessages { get; set; }
    }
}
