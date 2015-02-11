using System.Linq;
using MyFirstProject.Entities;
using MyFirstProject.Repository;

namespace Tests
{
    public class Mock : Repository
    {
        public override void Initialize()
        {            
            for (var i = 0; i < 5; i++)
            {
                if (i == 4)
                {
                    var lastComment = SaveNewComment(i);
                    SaveNewArticle(lastComment, 0);
                    break;
                }

                var currentComment = SaveNewComment(i);
                SaveNewArticle(currentComment, i);
            }

            Save(new User(0));

            Save(new Review(1, "Test", Get<User>()[0], Get<Article>()[1], new Rating(3)));
            Get<Article>()[1].AddRating(Get<Review>()[0].Rating);
        }

        private BaseComment SaveNewComment(int id)
        {
            Save(new Comment(id));
            var comments = Get<BaseComment>();
            return comments.Last();
        }

        private void SaveNewArticle(BaseComment comment, int id)
        {
            comment.Article = new Article(id);            
            Save(comment.Article);
        }
    }
}
