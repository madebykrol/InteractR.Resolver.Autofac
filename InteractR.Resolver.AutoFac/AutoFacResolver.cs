using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using InteractR.Interactor;
using InteractR.Resolver;

namespace InteractorHub.Resolvers.AutoFac
{
    public class AutoFacResolver : IResolver
    {
        private readonly IContainer _container;
        public AutoFacResolver(IContainer container)
        {
            _container = container;
        }

        private T Resolve<T>() 
            => _container.Resolve<T>();

        public IInteractor<TUseCase, TOutputPort> ResolveInteractor<TUseCase, TOutputPort>(TUseCase useCase) where TUseCase : IUseCase<TOutputPort> 
            => Resolve<IInteractor<TUseCase, TOutputPort>>();

        public IReadOnlyList<IMiddleware<TUseCase, TOutputPort>> ResolveMiddleware<TUseCase, TOutputPort>(TUseCase useCase) where TUseCase : IUseCase<TOutputPort> 
            => _container.Resolve<IEnumerable<IMiddleware<TUseCase, TOutputPort>>>().ToList();

        public IReadOnlyList<IMiddleware> ResolveGlobalMiddleware() 
            => _container.Resolve<IEnumerable<IMiddleware>>().ToList();
    }
}
