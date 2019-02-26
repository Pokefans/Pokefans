using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Pokefans.Controllers
{
    /// <summary>
    /// Static file controller. Note that this is intended to be for debug
    /// purposes only, hence it's quite simple and utterly slow on large files.
    /// At the very least it servers me well enough to debug on mac / linux.
    /// </summary>
    public class StaticFileController : Controller
    {
        public ActionResult Files()
        {
            return fileServer(Request.RawUrl, "subdomains/files");

        }

        public ActionResult Static()
        {
            return fileServer(Request.RawUrl, "subdomains/static");
        }

        private ActionResult fileServer(string url, string prefix) {
            
			byte[] file = System.IO.File.ReadAllBytes(Path.Combine(prefix, url));

			string filetype = url.Split(new char[] { '.' }).Last();
			string mimetype = "application/octet-stream";

			switch (filetype)
			{
				case "jpg":
					mimetype = "image/jpeg";
					break;
				case "png":
					mimetype = "image/png";
					break;
				case "gif":
					mimetype = "image/gif";
					break;
				case "css":
					mimetype = "text/css";
					break;
				case "js":
					mimetype = "text/javascript";
					break;
			}

			return File(file, mimetype);
        }
    }
}
