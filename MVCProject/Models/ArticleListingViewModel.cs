using System.Collections.Generic;

namespace MVCProject.Models
{
    public class ArticleListingViewModel
    {
        public List<ArticleViewModel> Articles { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public string View { get; set; }
    }
}