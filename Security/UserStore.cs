// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Pokefans.Data;
using Pokefans.Caching;

namespace Pokefans.Security
{
    public class UserStore : IUserStore<User, int>, 
                      IUserPasswordStore<User, int>, 
                      IUserLoginStore<User, int>, 
                      IUserEmailStore<User, int>,
                      IUserLockoutStore<User, int>,
                      IQueryableUserStore<User, int>,
                      IUserRoleStore<User, int>,
                      IUserTwoFactorStore<User, int>,
                      IUserSecurityStampStore<User, int>
    {
        private Entities entities;
        private Cache cache;


        public UserStore() : this(new Entities(), null) { }

        public UserStore(Entities ents, Cache cache)
        {
            this.entities = ents;
        }

        public Task CreateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            entities.Users.Add(user);

            return entities.SaveChangesAsync();
        }

        public Task DeleteAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            entities.Users.Remove(user);

            return entities.SaveChangesAsync();
        }

        public Task<User> FindByIdAsync(int userId)
        {
            if (userId == null)
                throw new ArgumentNullException("userId");

            return entities.Users.FindAsync(userId);
        }

        public Task<User> FindByNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException("Null or empty argument: userId");

            return Task.FromResult<User>(entities.Users.Where(g => g.UserName == userName).FirstOrDefault());
        }

        public async Task UpdateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            entities.SetModified(user);

            await entities.SaveChangesAsync();
        }

        public void Dispose()
        {
            if(entities != null)
                entities.Dispose();
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult<string>(user.Password);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult<bool>(user.Password == null ? false : true);
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrEmpty(passwordHash))
                throw new ArgumentException("passwordHash cannot be null or empty");

            user.Password = passwordHash;

            if (entities.Users.Any(x => x.UserName == user.UserName))
            {
                // user is already in the database
                user.Password = passwordHash;

                entities.SetModified(user);
                return entities.SaveChangesAsync();
            }
            user.Password = passwordHash;
            return Task.Run(() => 1 + 1); // yay async
        }

        public Task AddLoginAsync(User user, UserLoginInfo login)
        {
            UserLoginProvider p = new UserLoginProvider()
            {
                UserId = user.Id,
                ProviderKey = login.ProviderKey,
                ProviderName = login.LoginProvider
            };

            entities.UserLoginProvides.Add(p);

            return entities.SaveChangesAsync();
        }

        public Task<User> FindAsync(UserLoginInfo login)
        {
            var userprovider = entities.UserLoginProvides.Where(g => g.ProviderKey == login.ProviderKey && g.ProviderName == login.LoginProvider).FirstOrDefault();

            if (userprovider == null)
                return Task.FromResult<User>(null);

            return Task.FromResult<User>(userprovider.User);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        {
            var result = (from s in entities.UserLoginProvides
                          where s.UserId == user.Id
                          select s).ToList().Select(g => new UserLoginInfo(g.ProviderName, g.ProviderKey)).ToList();
            return Task.FromResult<IList<UserLoginInfo>>(result);
        }

        public Task RemoveLoginAsync(User user, UserLoginInfo login)
        {
            var provider = entities.UserLoginProvides.Where(g => g.UserId == user.Id && g.ProviderKey == login.ProviderKey && g.ProviderName == g.ProviderName).FirstOrDefault();

            if(provider != null)
            {
                entities.UserLoginProvides.Remove(provider);
                return entities.SaveChangesAsync();
            }
            return Task.FromResult<object>(null);
        }

        public Task<User> FindByEmailAsync(string email)
        {
            return Task.FromResult<User>(entities.Users.Where(e => e.Email == email).FirstOrDefault());
        }

        public Task<string> GetEmailAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult<string>(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult<bool>(user.EmailConfirmed);
        }

        public Task SetEmailAsync(User user, string email)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.Email = email;

            entities.SetModified(user);

            return entities.SaveChangesAsync();
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.EmailConfirmed = confirmed;

            entities.SetModified(user); // ?

            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var count = entities.UserLogins.Where(g => g.Success == false).Count();

            return Task.FromResult<int>(count);
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult<bool>(user.IsLockedOut);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (user.LockedOutDate.HasValue && user.LockedOutDate.Value > DateTimeOffset.Now)
                return Task.FromResult<DateTimeOffset>(user.LockedOutDate.Value);
            
            return Task.FromResult<DateTimeOffset>(new DateTimeOffset());
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            user.AccessFailedCount++;
            entities.SetModified(user);

            entities.SaveChanges();

            return Task.FromResult<int>(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            user.AccessFailedCount = 0;
            entities.SetModified(user);

            return entities.SaveChangesAsync();
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.IsLockedOut = enabled;

            // in the registration process, this will be called before the user is added to the database (for whatever reason).
            // so we must see if our database *really* contains this user or we'd get an error.
            if (entities.Users.Any(g => g.Email == user.Email))
            {
                entities.SetModified(user);
                entities.SaveChanges();
            }

            return Task.FromResult<object>(null);
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            user.LockedOutDate = lockoutEnd;

            entities.SetModified(user);

            return entities.SaveChangesAsync();
        }

        public IQueryable<User> Users
        {
            get { return entities.Users.AsQueryable(); }
        }

        public Task AddToRoleAsync(User user, string roleName)
        {
            var perm = entities.Roles.Where(g => g.Name == roleName).FirstOrDefault();

            if(perm == null)
            {
                throw new InvalidOperationException(string.Format("There is no Role with Name {0}", roleName));
            }

            UserRole p = new UserRole()
            {
                PermissionId = perm.Id,
                UserId = user.Id
            };

            entities.UserRoles.Add(p);

            return entities.SaveChangesAsync();
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            var list = (from s in entities.Roles
                        join t in entities.UserRoles on s.Id equals t.PermissionId
                        where t.UserId == user.Id
                        select s.Name).ToList();

            return Task.FromResult<IList<string>>(list);
        }

        public Task<bool> IsInRoleAsync(User user, string roleName)
        {
            return Task.FromResult<bool>(user.IsInRole(roleName, cache, entities));
        }

        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            if (user.IsInRole(roleName, cache, entities))
            {
                var userperm = (from s in entities.UserRoles
                                join t in entities.Roles on s.PermissionId equals t.Id
                                where t.Name == roleName && s.UserId == user.Id
                                select s).FirstOrDefault();

                if(userperm != null)
                {
                    entities.UserRoles.Remove(userperm);
                    return entities.SaveChangesAsync();
                }
            }

            return Task.FromResult<object>(null);
        }

        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return Task.FromResult<bool>(user.TwoFactorEnabled);
        }

        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.TwoFactorEnabled = enabled;

            entities.SetModified(user);
            return entities.SaveChangesAsync();

        }

        public Task SetSecurityStampAsync(User user, string stamp)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(User user)
        {
            return Task.FromResult(user.SecurityStamp);
        }
    }
}
