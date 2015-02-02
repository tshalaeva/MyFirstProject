using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    public class Facade
    {
        public IRepository Repository {get; private set;}

        public Facade(IRepository repository)
        {
            Repository = repository;
            Repository.Initialize();
        }

        public List<BaseComment> FilterCommentsByArticle(Article article)
        {
            var comments = Repository.GetComments();
            List<BaseComment> result = (from comment in comments
                                     where comment.Article.Id == article.Id
                                     select comment).ToList();
            return result;
        }

        public List<Article> FilterArticlesByAuthor(Author author)
        {
            var articles = Repository.GetArticles();
            List<Article> result = (from article in articles
                                    where article.Author.Id == author.Id
                                    select article).ToList();
            return result;
        }
    }
}
