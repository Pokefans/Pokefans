using System;
using System.Collections.Generic;
using Pokefans.Data.Forum;
using Pokefans.Models;
using Pokefans.Security;

namespace Pokefans.Areas.forum.Models
{
    public class ViewForumViewModel
    {
        public ViewForumViewModel(ForumSecurity security, int BoardId) 
        {
            Announcements = new List<ViewForumThreadViewModel>();
            Sticky = new List<ViewForumThreadViewModel>();
            Threads = new List<ViewForumThreadViewModel>();

            CanWrite = security.CanWriteBoard(BoardId);
            CanModerate = security.CanModerateBoard(BoardId);
            CanManage = security.CanManageBoard(BoardId);
        }

        public Board Board { get; set; }

        public List<ViewForumThreadViewModel> Announcements { get; set; }
        public List<ViewForumThreadViewModel> Sticky { get; set; }
        public List<ViewForumThreadViewModel> Threads { get; set; }

        public bool CanWrite { get; set; }
        public bool CanModerate { get; set; }
        public bool CanManage { get; set; }

        public PaginationViewModel Pagination { get; set; }
    }
}
