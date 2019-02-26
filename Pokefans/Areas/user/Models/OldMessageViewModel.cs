using System;
using Pokefans.Data;
using Pokefans.Data.UserData;

namespace Pokefans.Areas.user.Models
{
    public class OldMessageViewModel
    {      
		public User User { get; set; }

		public PrivateMessage Message { get; set; }
    }
}
