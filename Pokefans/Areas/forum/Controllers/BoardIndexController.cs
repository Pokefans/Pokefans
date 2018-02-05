using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Pokefans.Data;
using Pokefans.Data.Forum;
using Pokefans.Caching;
using Microsoft.AspNet.Identity;
using Pokefans.Security;
using Pokefans.Areas.forum.Models;

namespace Pokefans.Controllers
{
    public class BoardIndexController : Controller
    {
        private Entities db;
        private Cache cache;
        private ForumSecurity security;

        public BoardIndexController(Cache c, Entities e)
        {
            db = e;
            cache = c;
            User currentUser = null;
            if (User.Identity.IsAuthenticated)
            {
                int currentUserId = User.Identity.GetUserId<int>();
                currentUser = db.Users.FirstOrDefault(g => g.Id == currentUserId);
            }
            security = new ForumSecurity(e, c, currentUser);
        }

        public ActionResult Index()
        {
            Dictionary<int, Board> tboards = db.Boards.Include("LastPost").Include("LastPost.Author").Where(x => x.ShowInParentBoard == true).OrderBy(g => g.Order).ToDictionary(x => x.Id);

            List<Board> boards = makeTree(tboards);

            BoardIndexViewModel vm = new BoardIndexViewModel()
            {
                Boards = boards
            };

            return View("~/Areas/forum/Views/BoardIndex/Index.cshtml", vm);
        }

        private List<Board> makeTree(Dictionary<int, Board> tboards)
        {
            List<Board> boards = new List<Board>();

            foreach (KeyValuePair<int, Board> b in tboards)
            {
                if (!security.CanReadBoard(b.Key))
                    continue;

                if (b.Value.Type == BoardType.Category && b.Value.ParentBoardId == null)
                {
                    boards.Add(b.Value);
                    continue;
                }
                if (b.Value.ParentBoardId.HasValue)
                {
                    tboards[b.Value.ParentBoardId.Value].Children.Add(b.Value);
                }
            }

            return boards;
        }
    }
}
