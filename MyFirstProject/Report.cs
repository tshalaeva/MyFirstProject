using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    class Report
    {
        public List<Comment> filterCommentsByArticle(List<Comment> comments, Article article)
        {
            List<Comment> result = new List<Comment>();
            for (int i = 0; i < comments.Count; i++)
            {
                if (comments[i].Article.Id == article.Id)
                {
                    result.Add(comments[i]);
                }
            }
            return result;
        }
    }
}
