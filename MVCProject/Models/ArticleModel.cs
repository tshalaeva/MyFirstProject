using System.Collections.Generic;
using ObjectRepository.Entities;

namespace MVCProject.Models
{
    public class ArticleModel
    {
        private readonly Article _mArticle; 

        public List<CommentModel> Comments { get; set; }

        public ArticleModel(Article article)
        {
            _mArticle = article;
        }

        public string Title
        {
            get { return _mArticle.Title; }            
        }

        public string Author
        {
            get { return _mArticle.Author.NickName; }                         
        }

        public string Content
        {
            get { return _mArticle.Content; }            
        }

        public int Id
        {
            get { return _mArticle.Id; }
        } 
    }

    public class ArticleViewModel
    {

        public ArticleViewModel()
        {
        }

        public string Title
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }
    }
}