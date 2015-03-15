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
    public class Singleton
    {
        private static Singleton s_instance;
        private readonly IContainer m_container;

        public static Singleton Instance
        {
            get
            {
                return s_instance ?? (s_instance = new Singleton());
            }
        }

        public IContainer Container
        {
            get { return m_container; }
        }

        private Singleton()
        {
            m_container = new Container();
            m_container.Configure(x => x.For<IRepository>().Use<Repository>());
            m_container.Configure(x => x.For<Repository>().Use<Repository>());
        }
        
    }      
}
