using System;
using System.Web.Mvc;

namespace Pokefans.Areas.forum
{
    public class forumAreaRegistration : AreaRegistration
    {
		public override string AreaName
		{
			get
			{
				return "fanart";
			}
		}

        public override void RegisterArea(AreaRegistrationContext context) {



        }

		public forumAreaRegistration()
        {
        }
    }
}
