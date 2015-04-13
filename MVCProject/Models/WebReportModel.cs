using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MVCProject.Models
{
    public class WebReportModel
    {
        private readonly List<SelectListItem> m_options;

        private string content;

        public List<SelectListItem> Options
        {
            get { return m_options; }
        }

        public SelectListItem SelectedOption { get; set; }

        public string Content
        {
            get;
            set;
        }

        public WebReportModel()
        {
            m_options = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Text = "Print Article Titles",
                    Value = "1",
                    Selected = true
                },
                new SelectListItem()
                {
                    Text = "Print Average Rating For Article",
                    Value = "2"
                },
                new SelectListItem()
                {
                    Text = "Print List Of Privilegies",
                    Value = "3"
                },
                new SelectListItem()
                {
                    Text = "Print List Of Comments For Articles",
                    Value = "4"
                },
                new SelectListItem()
                {
                    Text = "Print Entity Code For Each Comment",
                    Value = "5"
                },
                new SelectListItem()
                {
                    Text = "Print Random Article Id",
                    Value = "6"
                }
            };
        }

        public string GetSelectedOption()
        {
            foreach (var option in Options)
            {
                if (option.Selected)
                {
                    return option.Value;
                }
            }
            return "";
        }
    }
}