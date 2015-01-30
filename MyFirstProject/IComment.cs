using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    public interface IComment 
    {        
        string Content { get; set; }

        User User { get; set; }

        Article Article { get; set; }

        void Display();

        bool IsReview();
    }
}
