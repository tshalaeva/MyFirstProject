using System;
using ObjectRepository.Entities;
using StructureMap;

namespace DataAccessLayer.Repositories
{
    public class IocContainer
    {
        private static readonly Lazy<IContainer> SContainerLazy;

        public static StructureMap.IContainer Container
        {
            get
            {
                return SContainerLazy.Value;
            }
        }

        private IocContainer()
        {
        }

        static IocContainer()
        {
            SContainerLazy = new Lazy<IContainer>(InitializeContainer);            
        }

        private static IContainer InitializeContainer()
        {
            IContainer container = new Container();
            container.Configure(x => x.For<IRepository<User>>().Use<DbUserRepository>());
            container.Configure(y => y.For <IRepository<Article>>().Use<DbArticleRepository>());
            container.Configure(z => z.For<IRepository<BaseComment>>().Use<DbCommentRepository>());
            container.Configure(facade => facade.For<Facade>().Use<Facade>());            
            return container;
        }
    }    
}
