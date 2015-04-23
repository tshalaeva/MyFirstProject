namespace Dto.DtoEntities
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        //public Guid PrivilegiesId { get; set; }

        //public Guid AuthorId { get; set; }

        public string NickName { get; set; }

        public decimal Popularity { get; set; }

        public string Privilegies { get; set; }
    }
}
