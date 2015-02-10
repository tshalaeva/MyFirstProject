using MyFirstProject.Entities;
using MyFirstProject.Repository;

namespace Tests
{
    public class Mock : Repository
    {
        protected override void Initialize()
        {            
            for (var i = 0; i < 5; i++)
            {
                if (i == 4)
                {
                    Save(new Comment(i));
                    Get<BaseComment>()[i].Article = new Article(0);
                    Save(Get<BaseComment>()[i].Article);
                    break;
                }

                Save(new Comment(i));
                Get<BaseComment>()[i].Article = new Article(i);
                Save(Get<BaseComment>()[i].Article);
            }

            Save(new User(0));

            Save(new Review(1, "Test", Get<User>()[0], Get<Article>()[1], new Rating(3)));
            Get<Article>()[1].AddRating(Get<Review>()[0].Rating);
        }
    }
}
