using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    public class Facade
    {
        public List<IComment> FilterCommentsByArticle(IRepository repository, Article article)
        {
            var comments = repository.GetComments();
            List<IComment> result = (from comment in comments
                                     where comment.Article.Id == article.Id
                                     select comment).ToList();
            return result;
        }

        public List<Article> FilterArticlesByAuthor(IRepository repository, Author author)
        {
            var articles = repository.GetArticles();
            List<Article> result = (from article in articles
                                    where article.Author.Id == author.Id
                                    select article).ToList();
            return result;
        }
    }
}
