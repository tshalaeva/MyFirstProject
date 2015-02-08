namespace MyFirstProject.Entities
{
    public class Author : User
    {
        public Author(int id)
            : base(id)
        {
        }

        public string NickName { get; set; }
        
        public double Popularity { get; set; }        
    }
}
