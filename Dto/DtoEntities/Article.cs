using System.Collections.Generic;

namespace Dto.DtoEntities
{
    public class Article
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public int AuthorId { get; set; }

        public int Id { get; set; }

        public List<object> Ratings { get; set; } 
    }
}
