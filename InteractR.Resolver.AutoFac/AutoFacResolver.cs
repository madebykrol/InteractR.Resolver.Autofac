using System;
using System.Collections.Generic;
using Autofac;
using InteractorHub.Resolver;

namespace InteractorHub.Resolvers.AutoFac
{
    public class AutoFacResolver : IResolver
    {
        private readonly IContainer _container;
        public AutoFacResolver(IContainer container)
        {
            _container = container;
        }
        public TInteractor ResolveInteractor<TInteractor>()
        {
            return Resolve<TInteractor>();
        }

        public object ResolveInteractor(Type interactorType)
        {
            return Resolve(interactorType);
        }

        private T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        private object Resolve(Type t)
        {
            return _container.Resolve(t);
        }
    }
}
