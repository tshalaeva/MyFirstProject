namespace MyFirstProject.Entity
{
    public class Comment : BaseComment, IEntity
    {   
        public Comment(int id)
        {
            Id = id;
        }
    }
}
