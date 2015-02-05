using System.Collections.Generic;
using System.Linq;
using MyFirstProject.Entity;
using MyFirstProject.Repository;

namespace MyFirstProject
{
    public class Facade<T> where T : Repository.Repository
    {
    public Facade(T repository)
    {
        Repository = repository;
        Repository.Initialize();
    }

    public T Repository { get; private set; }

    public List<BaseComment> FilterCommentsByArticle(Article article)
    {
        List<BaseComment> comments = ((IRepository<BaseComment>)Repository).Get();
        var result = (from comment in comments
            where comment.Article.Id == article.Id
            select comment).ToList();
        return result;
    }

    public List<Article> FilterArticlesByAuthor(Author author)
    {
        List<Article> articles = ((IRepository<Article>)Repository).Get();
        var result = (from article in articles
            where article.Author.Id == author.Id
            select article).ToList();
        return result;
    }
}
}
