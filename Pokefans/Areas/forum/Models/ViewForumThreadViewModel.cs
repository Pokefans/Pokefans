using System;
using Pokefans.Data.Forum;

namespace Pokefans.Areas.forum.Models
{
    public class ViewForumThreadViewModel
    {
        public Thread Thread { get; set; }
        public Post LastPost { get; set; }
    }
}
