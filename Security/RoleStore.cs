// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Pokefans.Data;

namespace Pokefans.Security
{
    public class RoleStore : IRoleStore<Role, int>, IQueryableRoleStore<Role, int>
    {
        private Entities entities;

        public RoleStore() : this(new Entities()) { }

        public RoleStore(Entities ents)
        {
            this.entities = ents;
        }

        public IQueryable<Role> Roles
        {
            get { return entities.Roles.AsQueryable(); }
        }

        public Task CreateAsync(Role role)
        {
            entities.Roles.Add(role);
            
            return entities.SaveChangesAsync();
        }

        public Task DeleteAsync(Role role)
        {
            entities.Roles.Remove(role);

            return entities.SaveChangesAsync();
        }

        public Task<Role> FindByIdAsync(int roleId)
        {
            return entities.Roles.FindAsync(roleId);
        }

        public Task<Role> FindByNameAsync(string roleName)
        {
            var role = entities.Roles.Where(g => g.Name == roleName).FirstOrDefault();

            return Task.FromResult<Role>(role);
        }

        public Task UpdateAsync(Role role)
        {
            entities.SetModified(role);

            return entities.SaveChangesAsync();
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}
