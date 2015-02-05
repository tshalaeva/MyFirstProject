using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFirstProject;
using MyFirstProject.Entity;
using MyFirstProject.Repository;

namespace Tests
{
    public class FacadeRepository : Repository
    {
        public FacadeRepository()
        {
            Articles = new List<Article>();
            Users = new List<User>();
            Admins = new List<Admin>();
            Authors = new List<Author>();
            Comments = new List<BaseComment>();
        }

        public override void Initialize()
        {            
            for (var i = 0; i < 5; i++)
            {
                if (i == 4)
                {
                    Comments.Add(new Comment(i));
                    Comments[i].Article = new Article(0);
                    break;
                }

                Comments.Add(new Comment(i));
                Comments[i].Article = new Article(i);
            }
        }
    }
}
