namespace DataAccessLayer.DtoEntities
{
    public class DtoUser
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public string NickName { get; set; }

        public decimal Popularity { get; set; }

        public string Privilegies { get; set; }
    }
}
