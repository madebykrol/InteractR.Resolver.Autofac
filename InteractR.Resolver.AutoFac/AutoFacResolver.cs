using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Autofac;
using InteractR.Interactor;
using InteractR.Resolver;

namespace InteractorHub.Resolvers.AutoFac
{
    public class AutoFacResolver : IResolver
    {
        private readonly IComponentContext _componentContext;
        public AutoFacResolver(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        private T Resolve<T>() 
            => _componentContext.Resolve<T>();

        public IInteractor<TUseCase, TOutputPort> ResolveInteractor<TUseCase, TOutputPort>(TUseCase useCase) where TUseCase : IUseCase<TOutputPort> 
            => Resolve<IInteractor<TUseCase, TOutputPort>>();

        public IReadOnlyList<IMiddleware<TUseCase, TOutputPort>> ResolveMiddleware<TUseCase, TOutputPort>(TUseCase useCase) where TUseCase : IUseCase<TOutputPort> 
            => _componentContext.Resolve<IEnumerable<IMiddleware<TUseCase, TOutputPort>>>().ToList();

        public IReadOnlyList<IMiddleware> ResolveGlobalMiddleware() 
            => _componentContext.Resolve<IEnumerable<IMiddleware>>().ToList();
    }
}
