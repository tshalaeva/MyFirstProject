using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;
using StructureMap.Configuration;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace MyFirstProject.Repository
{
    class Singleton : Repository
    {
        private static Singleton instance;

        public static Singleton Instance
        {
            get
            {
                if(instance == null)
                {
                    var container = new Container();
                    container.Configure(x => x.For<IRepository>().Use<Repository>());
                    instance = container.GetInstance<Singleton>(); 
                }
                return instance;
            }
        }
    }
}
