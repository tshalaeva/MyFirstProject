using System.Collections.Generic;
using System.Linq;

namespace ObjectRepository.Entities
{
    public class Admin : User
    {
        public Admin(int id)
            : base(id)
        {
        }

        public Admin()
        {

        }

        public List<string> Privilegies { get; set; }

        public override string ToString()
        {
            return Privilegies.Aggregate("", (current, privilegy) => string.Format("{0},{1}", current, privilegy));
        }
    }
}
