using System;
using DataAccessLayer;
using DataAccessLayer.Repositories;
using ObjectRepository.Entities;
using StructureMap;

namespace FLS.MyFirstProject.Infrastructure
{
    public class IocContainer
    {
        private static readonly Lazy<IContainer> SContainerLazy;

        public static IContainer Container
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
            container.Configure(facade => facade.For<Facade>().Use<Facade>());    
              
            container.Configure(x => x.For<IRepository<User>>().Use<DbUserRepository>());
            container.Configure(y => y.For <IRepository<Article>>().Use<DbArticleRepository>());
            container.Configure(z => z.For<IRepository<BaseComment>>().Use<DbCommentRepository>());      
            container.Configure(i => i.For<DbAdminRepository>().Use<DbAdminRepository>());
            container.Configure(j => j.For<DbAuthorRepository>().Use<DbAuthorRepository>());
            container.Configure(k => k.For<DbReviewRepository>().Use<DbReviewRepository>());
            container.Configure(l => l.For<DbReviewTextRepository>().Use<DbReviewTextRepository>());

            container.Configure(ado => ado.For<AdoHelper>().Use<AdoHelper>());
            container.Configure(dto => dto.For<DtoMapper>().Use<DtoMapper>());
            return container;
        }
    }    
}
