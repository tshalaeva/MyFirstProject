using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    class Article
    {
        private string title;
        private string content;
        private Author author;
        private List<Comment> comments;
        private List<Rating>  rating;
        private int id;

        public Article(int idValue, Author authorValue, string titleValue, string contentValue)
        {
            id = idValue;
            title = titleValue;
            content = contentValue;
            author = authorValue;

        }

        public string getTitle()
        {
            return title;
        }

        public void setTitle(string titleValue)
        {
            title = titleValue;
        }

        public string getContent()
        {
            return content;
        }

        public void setContent(string contentValue)
        {
            content = contentValue;
        }

        public Author getAuthor()
        {
            return author;
        }

        public void setAuthor(Author authorValue)
        {
            author = authorValue;
        }

        public List<Comment> getComments()
        {
            return comments;
        }

        public void addComment(Comment commentValue)
        {
            comments.Add(commentValue);
        }

        public void setRating(Rating ratingValue)
        {
            bool flag = false;
            for(int i = 0; i < rating.Count; i++)
            {
                if (rating[i].getUser().getId() == ratingValue.getUser().getId())
                {
                    rating[i].setRating(ratingValue.getRating());
                    flag = true;
                    break;
                }
            }
            if (flag == false)
            {
                rating.Add(ratingValue);
            }
        }

        public int getAverageRating()
        {
            int sum = 0;
            for (int i = 0; i < rating.Count; i++)
            {
                sum = sum + rating[i].getRating();
            }
            if (rating.Count != 0)
            {
                return sum / rating.Count;
            }
            else
            {
                return 0;
            }
        }

        public int getId()
        {
            return id;
        }
    }
}
