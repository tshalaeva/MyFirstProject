using ObjectRepository.Entities;

namespace MVCProject.Adapters
{
    public class ArticleAdapter
    {
        private readonly Article _mArticle;
        
        public ArticleAdapter(Article article)
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
}
