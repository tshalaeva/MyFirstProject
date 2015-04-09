using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFirstProject.Entities;

namespace MyFirstProject.Repository
{
    class DbUserRepository : IRepository<User>
    {
        private readonly AdoHelper _adoHelper;

        public DbUserRepository()
        {
            _adoHelper = new AdoHelper();
        }

        public bool Initialized
        {
            get
            {
                if (_adoHelper.GetUsersCount() != 0)
                {
                    return true;
                }
                return false;
            }
        }

        public List<User> Get()
        {
            return _adoHelper.GetUsers();
        }

        public int Save(User entity)
        {
            if (!(entity is Admin) && !(entity is Author))
            {
                return _adoHelper.SaveUser(entity);
            }
            if (entity is Admin)
            {
                return _adoHelper.SaveAdmin((Admin) entity);
            }
            return _adoHelper.SaveAuthor((Author) entity);
        }

        public void Delete(User entity)
        {
            _adoHelper.DeleteUser(entity);
        }

        public void Update(User existingEntity, User newEntity)
        {
            _adoHelper.UpdateUser(existingEntity, newEntity);
        }

        public User GetById(int? id)
        {
            return _adoHelper.GetUserById(id);
        }

        public User GetRandom()
        {
            return _adoHelper.GetRandomUser();
        }
    }

    class DbArticleRepository : IRepository<Article>
    {
        private readonly AdoHelper _adoHelper;

        public DbArticleRepository()
        {
            _adoHelper = new AdoHelper();
        }

        public bool Initialized
        {
            get
            {
                if (_adoHelper.GetArticlesCount() != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<Article> Get()
        {
            return _adoHelper.GetArticles();
        }

        public int Save(Article entity)
        {
            return _adoHelper.SaveArticle(entity);
        }

        public void Delete(Article entity)
        {
            _adoHelper.DeleteArticle(entity);
        }

        public void Update(Article existingEntity, Article newEntity)
        {
            _adoHelper.UpdateArticle(existingEntity, newEntity);
        }

        public Article GetById(int? id)
        {
            return _adoHelper.GetArticleById(id);
        }

        public Article GetRandom()
        {
            return _adoHelper.GetRandomArticle();
        }
    }

    class DbCommentRepository : IRepository<BaseComment>
    {
        private AdoHelper _adoHelper;
        public DbCommentRepository()
        {
            _adoHelper = new AdoHelper();
        }

        public bool Initialized
        {
            get
            {
                if (_adoHelper.GetCommentsCount() != 0)
                {
                    return true;
                }
                return false;
            }
        }

        public void Update(BaseComment oldComment, BaseComment newComment)
        {

        }

        public int Save(BaseComment comment)
        {
            if (!(comment is Review) && !(comment is ReviewText))
            {
                return _adoHelper.SaveComment((Comment)comment);
            }
            else
            {
                if (comment is ReviewText)
                {
                    return _adoHelper.SaveReviewText((ReviewText)comment);
                }
                return _adoHelper.SaveReview((Review)comment);
            }
        } 
       
        public BaseComment GetRandom()
        {
            return _adoHelper.GetRandomComment();
        }

        public BaseComment GetById(int? id)
        {
            return _adoHelper.GetCommentById(id);
        }

        public List<BaseComment> Get()
        {
            return _adoHelper.GetComments();
        }

        public void Delete(BaseComment comment)
        {
            _adoHelper.DeleteComment(comment);
        }
    }
}
