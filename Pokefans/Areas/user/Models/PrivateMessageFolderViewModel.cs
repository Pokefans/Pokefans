using System;
using System.Collections.Generic;
using Pokefans.Data.UserData;

namespace Pokefans.Areas.user.Models
{
    public class PrivateMessageFolderViewModel<T>
    {
        public PrivateMessageFolderViewModel()
        {
            Messages = new List<T>();
            Labels = new Dictionary<int, PrivateMessageLabel>();
            HasMore = false;
        }

        public IEnumerable<T> Messages { get; set; }
        public Dictionary<int, PrivateMessageLabel> Labels { get; set; }

        public bool HasMore { get; set; }
    }
}
