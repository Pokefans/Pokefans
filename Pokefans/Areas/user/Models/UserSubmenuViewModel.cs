// Copyright 2017 the pokefans authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pokefans.Areas.user.Models
{
	public class UserSubmenuViewModel
	{
		public string ActiveMenuKey { get; set; }

		public string SearchTerm { get; set; }

		public UserSubmenuViewModel(string menu, string term)
		{
			ActiveMenuKey = menu;
			SearchTerm = term;
		}
	}
}