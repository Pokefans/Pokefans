using System;
using Pokefans.Data.Forum;

namespace Pokefans.Areas.forum.Models
{
    public class NewPostViewModel : NewThreadViewModel
    {
        public Thread Thread { get; set; }
    }
}
