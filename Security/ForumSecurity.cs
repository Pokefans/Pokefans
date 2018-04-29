using System;
using System.Collections.Generic;
using Pokefans.Caching;
using Pokefans.Data;
using Pokefans.Data.Forum;
using System.Linq;

namespace Pokefans.Security
{
    public class ForumSecurity
    {
        private Entities db;
        private Cache cache;
        private bool hasBeenLoaded = false;

        private User user;

        public User User
        {
            get
            {
                return user;
            }
        }

        private BoardPermissions guest;
        private Dictionary<int, BoardPermissions> guestBoards;
        private BoardPermissions normal;
        private Dictionary<int, BoardPermissions> normalBoards;
        private List<BoardPermissions> roles;
        private Dictionary<int, List<BoardPermissions>> rolesBoards;
        private List<BoardPermissions> groups;
        private Dictionary<int, List<BoardPermissions>> groupsBoards;

        private enum AccessLevel { Read, Write, Moderate, Manage };

        public ForumSecurity(Entities entities, Cache c, User u)
        {
            db = entities;
            user = u;
            cache = c;
            // TODO: Cache until manual refresh or logout.
        }

        private bool checkAccess(int BoardId, AccessLevel level) 
        {
            BoardAccess access = BoardAccess.Default;

            if (!hasBeenLoaded) Refresh();

            if (User == null)
            {
                access = setIfHigher(access, checkBoardAccess(guest, level));
                if (guestBoards.ContainsKey(BoardId)) access = setIfHigher(access, checkBoardAccess(guestBoards[BoardId], level));
            }
            else
            {
                // if we are superadmin, any role checking is superficient.
                // we can do everything, and this cannot be overridden.
                // note that the normal role chain does not apply to board
                // permissions.
                if (User.IsInRole("superadmin", cache, db))
                    return true;

                access = setIfHigher(access, checkBoardAccess(normal, level));
                if (normalBoards.ContainsKey(BoardId)) access = setIfHigher(access, checkBoardAccess(normalBoards[BoardId], level));

                foreach (BoardPermissions r in roles)
                {
                    access = setIfHigher(access, checkBoardAccess(r, level));
                    // we can allow us a short cut here
                    if (access == BoardAccess.Never)
                        return false;
                }

                if (rolesBoards.ContainsKey(BoardId))
                    foreach (BoardPermissions r in rolesBoards[BoardId])
                    {
                        access = setIfHigher(access, checkBoardAccess(r, level));
                        // we can allow us a short cut here
                        if (access == BoardAccess.Never)
                            return false;
                    }

                foreach (BoardPermissions r in groups)
                {
                    access = setIfHigher(access, checkBoardAccess(r, level));
                    // we can allow us a short cut here
                    if (access == BoardAccess.Never)
                        return false;
                }

                if (groupsBoards.ContainsKey(BoardId))
                    foreach (BoardPermissions r in groupsBoards[BoardId])
                    {
                        access = setIfHigher(access, checkBoardAccess(r, level));
                        // we can allow us a short cut here
                        if (access == BoardAccess.Never)
                            return false;
                    }
            }


            if (access == BoardAccess.Allow || access == BoardAccess.Always)
                return true;

            return false;
        }

        private BoardAccess checkBoardAccess(BoardPermissions perms, AccessLevel level) 
        {
            switch(level) 
            {
                case AccessLevel.Read:     return perms.CanRead;
                case AccessLevel.Write:    return perms.CanWrite;
                case AccessLevel.Moderate: return perms.CanModerate;
                case AccessLevel.Manage:   return perms.CanManage;
                default: throw new ArgumentException("unknown AccessLevel");
            }

        }

        // With the generic helper method aboe, we can now just alias
        // explicit CanXXXBoard functions to the checkAccess function, which is
        // pretty neat if you ask me.
        public bool CanReadBoard(int BoardId) => checkAccess(BoardId, AccessLevel.Read);
        public bool CanWriteBoard(int BoardId) => checkAccess(BoardId, AccessLevel.Write);
        public bool CanModerateBoard(int BoardId) => checkAccess(BoardId, AccessLevel.Moderate);
        public bool CanManageBoard(int BoardId) => checkAccess(BoardId, AccessLevel.Manage);

        private BoardAccess setIfHigher(BoardAccess current, BoardAccess target)
        {
            if (target == BoardAccess.Default || current == BoardAccess.Never)
                return current;

            if (target == BoardAccess.Always && current != BoardAccess.Never)
                return target;

            if (target == BoardAccess.Allow && current == BoardAccess.Default)
                return target;

            if (target == BoardAccess.Deny && current != BoardAccess.Always)
                return target;

            if (target == BoardAccess.Never)
                return target;

            throw new Exception("fakup in permission checking");
        }

        public void Refresh()
        {
            // guest is always loaded.
            guest = db.BoardPermissions.First(g => g.Permissionset == BoardPermissionsets.Guest && g.BoardId == null);

            guestBoards = db.BoardPermissions.Where(g => g.Permissionset == BoardPermissionsets.Guest && g.BoardId != null).ToDictionary(g => g.BoardId.Value);

            if (User != null)
            {
                // if we provide a user, load their permissions too.
                normal = db.BoardPermissions.First(g => g.Permissionset == BoardPermissionsets.Default && g.BoardId == null);
                normalBoards = db.BoardPermissions.Where(g => g.Permissionset == BoardPermissionsets.Default && g.BoardId != null).ToDictionary(g => g.BoardId.Value);

                var troles = db.UserRoles.Where(g => g.UserId == User.Id).Select(x => x.PermissionId).ToList();

                roles = db.BoardPermissions.Where(g => g.Permissionset == BoardPermissionsets.Role && troles.Contains(g.RoleId.Value) && g.BoardId == null).ToList();
                rolesBoards = db.BoardPermissions.Where(g => g.Permissionset == BoardPermissionsets.Role && troles.Contains(g.RoleId.Value) && g.BoardId != null).GroupBy(g => g.BoardId.Value).ToDictionary(g => g.Key, g => g.ToList());

                var tgroups = db.ForumGroupsUsers.Where(g => g.UserId == User.Id).Select(g => g.GroupId).ToList();

                groups = db.BoardPermissions.Where(g => g.Permissionset == BoardPermissionsets.Group && tgroups.Contains(g.GroupId.Value) && g.BoardId == null).ToList();
                groupsBoards = db.BoardPermissions.Where(g => g.Permissionset == BoardPermissionsets.Group && tgroups.Contains(g.GroupId.Value) && g.BoardId != null).GroupBy(g => g.BoardId.Value).ToDictionary(g => g.Key, g => g.ToList());
            }

        }
    }
}
