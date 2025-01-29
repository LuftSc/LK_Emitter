using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.SharedKernal.Storage
{
    public static class RepositoryExtensions
    {
        public static RepositoryRegistrar<TRepository> RegisterRepository<TRepository, TRepositoryImpl>(this
            IServiceCollection serviceCollection)
            where TRepository : class
            where TRepositoryImpl : TRepository
        {
            var repositoryType = typeof(TRepository);
            var readonlyImplementation = repositoryType.GetInterface(typeof(IReadOnlyRepository<>).Name)!;

            serviceCollection.RegisterRepositoryInternal(repositoryType
                , readonlyImplementation
                , typeof(TRepositoryImpl));

            return new RepositoryRegistrar<TRepository>(serviceCollection);
        }

        public static RepositoryRegistrar<TRepository> RegisterRepository
            <TRepository, TReadOnlyRepository, TRepositoryImpl>(
            this IServiceCollection serviceCollection)
            where TRepository : class
            where TReadOnlyRepository : class
            where TRepositoryImpl : TRepository
        {
            serviceCollection.RegisterRepositoryInternal(typeof(TRepository)
                , typeof(TReadOnlyRepository)
                , typeof(TRepositoryImpl));

            return new RepositoryRegistrar<TRepository>(serviceCollection);
        }

        private static void RegisterRepositoryInternal(this IServiceCollection serviceCollection
            , Type repository
            , Type readonlyRepository
            , Type repositoryImpl)
        {
            serviceCollection.AddTransient(repositoryImpl);
            serviceCollection.AddTransient(repository, repositoryImpl);
            serviceCollection.AddTransient(readonlyRepository, x =>
            {
                var repImpl = x.GetRequiredService(repositoryImpl);

                var property = repImpl.GetType().GetProperty(nameof(IReadOnlyRepository<IAggregateRoot>.ReadOnly))!;
                property.SetValue(repImpl, true);

                return repImpl;
            });
        }
        public class RepositoryRegistrar<TRepository>
            where TRepository : class
        {
            private readonly IServiceCollection serviceCollection;

            public RepositoryRegistrar(IServiceCollection serviceCollection)
            {
                this.serviceCollection = serviceCollection;
            }

            public RepositoryRegistrar<TRepository> AddDecorator<TDecorator>()
                where TDecorator : class, TRepository
            {
                serviceCollection.Replace(ServiceDescriptor.Transient<TRepository, TDecorator>());
                return this;
            }
            public RepositoryRegistrar<TRepository> AddDecorator<TDecorator>(Func<IServiceProvider, TDecorator> factory)
                where TDecorator : class, TRepository
            {
                serviceCollection.AddTransient<TRepository, TDecorator>(factory);
                return this;
            }
        }
    }
}
