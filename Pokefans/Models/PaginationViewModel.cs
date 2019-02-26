using System;
namespace Pokefans.Models
{
    public class PaginationViewModel
    {
        public PaginationViewModel(int min, int max, int current, string urlTemplate, int itemsPerPage)
        {
            Minimum = min;
            Maximum = max;
            Current = current;
            UrlTemplate = urlTemplate;
            ItemsPerPage = itemsPerPage;
        }

        public int Minimum { get; set; }

        public int Maximum { get; set; }

        public int Current { get; set; }

        public string UrlTemplate { get; set; }

        public int ItemsPerPage { get; set; }

        /// <summary>
        /// Gets the URL for the specific page. While the Page is 1-indexed,
        /// the page itself is not, because page 1 starts at start=0, page 2 at 1*itemsPerPage and so on.
        /// </summary>
        /// <returns>The URL.</returns>
        /// <param name="page">Page.</param>
        public string GetUrl(int page) => string.Format(UrlTemplate, (page - 1) * ItemsPerPage);
    }
}
