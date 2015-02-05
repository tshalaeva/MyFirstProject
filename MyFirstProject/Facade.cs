using System.Collections.Generic;
using System.Linq;
using MyFirstProject.Entity;
using MyFirstProject.Repository;

namespace MyFirstProject
{
    public class Facade
    {
        public Facade(IBaseRepository repository)
        {
            Repository = repository;
            Repository.Initialize();
        }

        public IBaseRepository Repository { get; private set; }
       
        public List<BaseComment> FilterCommentsByArticle(Article article)
        {
            var comments = Repository.GetComments();            
            var result = (from comment in comments
                                     where comment.Article.Id == article.Id
                                     select comment).ToList();            
            return result;
        }

        public List<Article> FilterArticlesByAuthor(Author author)
        {
            var articles = Repository.GetArticles();
            var result = (from article in articles
                                    where article.Author.Id == author.Id
                                    select article).ToList();
            return result;
        }
    }
}
