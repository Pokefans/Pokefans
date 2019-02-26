// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System;
using System.Text.RegularExpressions;
using PagedList;
using Pokefans.Data;
using System.Collections.Generic;

namespace Pokefans.Areas.mitarbeit.Models
{
    public class ContentListViewModel
    {
        public IPagedList<Content> Contents { get; set; }

        public string Filter { get; set; }

        public string AdditionalFilter { get; set; }

        public int? Page { get; set; }

        public static int PageSize
        {
            get { return 20; }
        }

        /// <summary>
        /// Checks weither or not a special filter is included in the filter list
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public bool IncludesFilter(string filter)
        {
            if (string.IsNullOrEmpty(Filter))
            {
                return false;
            }

            var tokens = filter.Split(':');
            var regex = String.Format(@"{0}:([^\| ]+\|?)*{1}", tokens[0], tokens[1]);

            return Regex.IsMatch(Filter, regex);
        }
    }
}