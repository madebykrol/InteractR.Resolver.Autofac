using System;
using System.Collections.Generic;
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
        {
            return _container.Resolve<T>();
        }
        public IInteractor<TUseCase, TOutputPort> ResolveInteractor<TUseCase, TOutputPort>(TUseCase useCase) where TUseCase : IUseCase<TOutputPort>
        {
            return Resolve<IInteractor<TUseCase, TOutputPort>>();
        }
    }
}
