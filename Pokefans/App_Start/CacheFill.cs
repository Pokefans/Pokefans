// Copyright 2015-2016 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pokefans.Data;
using Pokefans.Caching;

namespace Pokefans.App_Start
{
    public class CacheFill
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheFill"/> class. The cache will be filled 
        /// </summary>
        /// <param name="ents">The ents.</param>
        /// <param name="cache">The cache.</param>
        public CacheFill(Entities ents, Cache cache)
        {
            // UserNoteActions, not user selectable
            Dictionary<string, int> systemUserNoteActions = new Dictionary<string, int>();

            ents.UserNoteActions.Where(x => x.IsUserSelectable == false).ToList().ForEach(x => systemUserNoteActions.Add(x.CodeHandle, x.Id));

            cache.Add<Dictionary<string, int>>("SystemUserNoteActions", systemUserNoteActions);

            // User Note Actions, user selectable
            Dictionary<int, string> userUserNoteActions = new Dictionary<int, string>();

            ents.UserNoteActions.Where(x => x.IsUserSelectable == true).ToList().ForEach(x => userUserNoteActions.Add(x.Id, x.Name));

            cache.Add<Dictionary<int, string>>("UserUserNoteActions", userUserNoteActions);

            // All User Note Actions
            Dictionary<int, string> userNoteActions = new Dictionary<int, string>();

            ents.UserNoteActions.ToList().ForEach(x => userNoteActions.Add(x.Id, x.Name));

            cache.Add<Dictionary<int, string>>("UserNoteActions", userNoteActions);

            // Advertising Forms
            Dictionary<int, string> advertisingForms = new Dictionary<int, string>();

            var adlist = ents.UserAdvertisingForms.ToList();
            adlist.ForEach(x => advertisingForms.Add(x.Id, x.Name));

            cache.Add<Dictionary<int, string>>("AdvertisingForms", advertisingForms);
            cache.Add<List<UserAdvertisingForm>>("AdvertisingFormsFull", adlist);

            // BVS-Visible roles
            Dictionary<int, string> bvsRoles = new Dictionary<int, string>();

            ents.Roles.Where(x => x.Name == "global-moderator" || 
                             x.Name == "bereichsassistent" || 
                             x.Name == "bereichsleiter" || 
                             x.Name == "administrator" || 
                             x.Name == "super-admin")
                      .ToList()
                      .ForEach(x => bvsRoles.Add(x.Id, x.FriendlyName));

            cache.Add<Dictionary<int, string>>("BvsRoles", bvsRoles);

            // Fanart-Categories
            Dictionary<int, string> fanartCategories = new Dictionary<int, string>();

            var catlist = ents.FanartCategories.ToList();
            catlist.ForEach(x => fanartCategories.Add(x.Id, x.Name));
            cache.Add("FanartCategories", fanartCategories);

            // Fanart-Category-Urls

            Dictionary<int, string> fanartUrls = new Dictionary<int, string>();
            catlist.ForEach(x => fanartUrls.Add(x.Id, x.Uri));
            cache.Add("FanartUrls", fanartUrls);

        }
    }
}