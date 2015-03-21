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
    public class IocContainer
    {
        private static readonly Lazy<IContainer> s_containerLazy;

        public static IContainer Container
        {
            get
            {
                return s_containerLazy.Value;
            }
        }

        static IocContainer()
        {
            s_containerLazy = new Lazy<IContainer>(InitializeContainer);            
        }

        private static IContainer InitializeContainer()
        {
            IContainer container = new Container();
            container.Configure(x => x.For<IRepository>().Use<Repository>());            
            return container;
        }
    }    
}
