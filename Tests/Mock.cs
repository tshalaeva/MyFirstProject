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
                    Save(new Comment(i));
                    Get<BaseComment>()[i].Article = new Article(0);
                    break;
                }

                Save(new Comment(i));
                Get<BaseComment>()[i].Article = new Article(i);
            }
        }
    }
}
