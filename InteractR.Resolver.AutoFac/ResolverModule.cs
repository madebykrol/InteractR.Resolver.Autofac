using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using InteractorHub.Resolvers.AutoFac;

namespace InteractR.Resolver.AutoFac
{
    public class ResolverModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Hub>().As<IInteractorHub>();
            builder.Register(c =>
            {
                var context = c.Resolve<IComponentContext>();
                return new AutoFacResolver(context);
            }).As<IResolver>();
        }
    }
}
