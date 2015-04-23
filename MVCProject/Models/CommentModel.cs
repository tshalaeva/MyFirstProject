using System.Web.Providers.Entities;
using MVCProject.Adapters;
using ObjectRepository.Entities;

namespace MVCProject.Models
{
    public class CommentModel
    {
        private readonly CommentAdapter _mAdapter;

        public int Id
        {
            get { return _mAdapter.Id; }            
        }

        public ObjectRepository.Entities.User User { get { return _mAdapter.User; } }

        public ArticleModel Article 
        {
            get { return new ArticleModel(new ArticleAdapter(_mAdapter.Article)); }
        }

        public string Content 
        {
            get { return _mAdapter.Content; }
        }

        public CommentModel(CommentAdapter comment)
        {
            _mAdapter = comment;
        }

        public string Show()
        {
            return _mAdapter.ToString();
        }
    }
}