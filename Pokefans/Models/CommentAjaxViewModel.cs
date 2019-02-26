using Pokefans.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pokefans.Models
{
    public class CommentAjaxViewModel
    {
        public CommentViewModel Comment { get; set; }

        public bool Success { get; set; }
    }
}