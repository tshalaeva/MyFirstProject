namespace ObjectRepository.Entities
{
    public class Author : User
    {
        public Author(int id)
            : base(id)
        {
        }

        public Author()
        {
        }

        public string NickName { get; set; }
        
        public decimal Popularity { get; set; }        
    }
}
