using System.ComponentModel.DataAnnotations;

namespace MVCProject.Models
{
    public class UserViewModel
    {
        public UserViewModel()
        {
        }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Range(1,150)]
        public int Age { get; set; }
    }
}