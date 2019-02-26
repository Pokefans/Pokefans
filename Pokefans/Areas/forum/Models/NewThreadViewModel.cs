using System;
using Pokefans.Data.Forum;

namespace Pokefans.Areas.forum.Models
{
    public class NewThreadViewModel
    {
        public Board Board { get; set; }

        public ThreadType Type { get; set; }

        public string Body { get; set; }

        public string Subject { get; set; }

        public bool Watch { get; set; }
    }
}
