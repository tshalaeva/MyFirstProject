using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject.Entities.Dto
{
    class Comment
    {
        public Comment() { }

        public string Content { get; set; }

        public int UserId { get; set; }

        public int ArticleId { get; set; }

        public int Id { get; set; }

        public Guid RatingId { get; set; }
    }
}
