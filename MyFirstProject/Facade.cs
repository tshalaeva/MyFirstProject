using System.Collections.Generic;
using System.Linq;
using MyFirstProject.Entities;

namespace MyFirstProject
{
    public class Facade<T> where T : IEntity
    {
        public Facade()
        {
            Repository = new Repository.Repository();
            Repository.Initialize();
        }

        public Facade(Repository.Repository repository)
        {
            Repository = repository;
            Repository.Initialize();
        }

        private Repository.Repository Repository { get; set; }

        public void Save(T entity)
        {
            Repository.Save(entity);
        }

        public List<T> Get()
        {
            return Repository.Get<T>();
        }

        public void Delete(T entity)
        {
            Repository.Delete(entity);
        }

        public List<BaseComment> FilterCommentsByArticle(Article article)
        {            
            var comments = Repository.Get<BaseComment>();
            var result = (from comment in comments
                where comment.Article.Id == article.Id
                select comment).ToList();
            return result;
        }
    }
}
