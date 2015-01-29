using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    interface IComment
    {
        Article Article { get; set; }

        void display();        
    }
}
