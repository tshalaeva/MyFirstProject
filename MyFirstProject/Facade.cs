using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    public class Facade
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

        public List<Article> filterArticlesByAuthor(List<Article> articles, Author author)
        {
            List<Article> result = new List<Article>();
            for (int i = 0; i < articles.Count; i++)
            {
                if (articles[i].Author.Id == author.Id)
                {
                    result.Add(articles[i]);
                }
            }
            return result;
        }
    }
}
