using System.Collections.Generic;

namespace DataAccessLayer.DtoEntities
{
    public class DtoArticle
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public int AuthorId { get; set; }

        public string AuthorFirstName { get; set; }

        public string AuthorLastName { get; set; }

        public int AuthorAge { get; set; }

        public string AuthorNickName { get; set; }

        public decimal AuthorPopularity { get; set; }

        public int Id { get; set; }

        public List<object> Ratings { get; set; } 
    }
}
