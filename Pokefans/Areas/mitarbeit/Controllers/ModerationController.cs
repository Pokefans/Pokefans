using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Pokefans.Data;

namespace Pokefans.Controllers
{
    public partial class ModerationController : Controller
    {

        private Entities db;

        public ModerationController(Entities entities) 
        {
            db = entities;    
        }
    }
}
