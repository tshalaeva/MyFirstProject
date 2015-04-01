using System;
using StructureMap;

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

        private IocContainer()
        {
        }

        static IocContainer()
        {
            s_containerLazy = new Lazy<IContainer>(InitializeContainer);            
        }

        private static IContainer InitializeContainer()
        {
            IContainer container = new Container();
            container.Configure(x => x.For<IRepository>().Use<Repository>());                
            container.Configure(x => x.For<ArticleRepository>().Use<ArticleRepository>()); 
            container.Configure(y => y.For<UserRepository>().Use<UserRepository>());
            container.Configure(z => z.For<CommentRepository>().Use<CommentRepository>());            
            container.Configure(facade => facade.For<Facade>().Use<Facade>());
            return container;
        }
    }    
}
