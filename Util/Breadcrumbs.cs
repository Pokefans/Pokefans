using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebPages.Html;




namespace Pokefans.Util
{
    public class Breadcrumbs : IBreadcrumbs
    {
        private List<Breadcrumb> crumbs;

        public Breadcrumbs()
        {
            this.crumbs = new List<Breadcrumb>();
            this.Add("Pokefans", "Home", "Index", new { area = ""});
        }

        /// <summary>
        /// Gets the current breadcrumbs
        /// </summary>
        /// <value>
        /// The current breadcrumbs
        /// </value>
        public List<Breadcrumb> Current
        {
            get { return crumbs; }
        }

        /// <summary>
        /// Add a Breadcrumb which will use a Link to the current Page.
        /// </summary>
        /// <param name="text">The text.</param>
        public void Add(string text)
        {
            crumbs.Add(new Breadcrumb
            {
                Text = text
            });
        }

        /// <summary>
        /// Add a Breadcrumb linking to another page in the current Area
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        public void Add(string text, string controller, string action)
        {
            crumbs.Add(new Breadcrumb
                {
                    Text = text,
                    Controller = controller,
                    Action = action
                });
        }

        /// <summary>
        /// Add a Breadcrumb, most generic version.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        /// <param name="routeValues">The route values.</param>
        public void Add(string text, string controller, string action, object routeValues)
        {
            crumbs.Add(new Breadcrumb
            {
                Text = text,
                Controller = controller,
                Action = action,
                RouteValues = routeValues
            });
        }
    }
    public class Breadcrumb
    {
        /// <summary>
        /// Gets or sets the controller.
        /// </summary>
        /// <value>
        /// The controller.
        /// </value>
        public string Controller { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the route values.
        /// </summary>
        /// <value>
        /// The route values.
        /// </value>
        public object RouteValues { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; }
    }
}
