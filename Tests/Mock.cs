using System.Linq;
using MyFirstProject.Entities;
using System.Collections.Generic;
using MyFirstProject.Repository;

namespace Tests
{
    public class Mock : Repository
    {    
        public Mock()
        {
            m_data = new List<IEntity>() { new Article(0), new Article(1), new Article(2), new Article(2), new User(0), new User(1) };
            Save(new Comment(0, "Content 0", Get<User>().First(), Get<Article>().First()));
            Save(new Comment(1, "Content 1", Get<User>()[1], Get<Article>()[0]));
            Save(new Review(0, "Review 0", Get<User>().First(), Get<Article>().First(), new Rating(5)));
            Save(new ReviewText(0, "Review Text 0", Get<User>().First(), Get<Article>().First(), new Rating(1)));
        }
    }
}
