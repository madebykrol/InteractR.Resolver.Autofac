using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using InteractorHub.Resolvers.AutoFac;
using InteractR;
using InteractR.Interactor;
using InteractR.Resolver.AutoFac.Tests.Mocks;
using NSubstitute;
using NUnit.Framework;

namespace InteractorHub.Tests.Resolvers.AutoFac
{
    [TestFixture]
    public class AutoFacTests
    {
        private IContainer _container;
        private IInteractorHub _interactorHub;

        private IInteractor<MockUseCase, IMockOutputPort> _useCaseInteractor;

        private IMiddleware<MockUseCase, IMockOutputPort> _middleware1;
        private IMiddleware<MockUseCase, IMockOutputPort> _middleware2;

        private IMiddleware _globalMiddleware;

        [SetUp]
        public void Setup()
        {
            var builder = new ContainerBuilder();
            _useCaseInteractor = Substitute.For<IInteractor<MockUseCase, IMockOutputPort>>();

            _middleware1 = Substitute.For<IMiddleware<MockUseCase, IMockOutputPort>>();
            _middleware1.Execute(
                    Arg.Any<MockUseCase>(),
                    Arg.Any<IMockOutputPort>(),
                    d => Task.FromResult(new UseCaseResult(true)),
                    Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(x => new UseCaseResult(true))
                .AndDoes(x => x.Arg<Func<MockUseCase, Task<UseCaseResult>>>().Invoke(x.Arg<MockUseCase>()));

            _middleware2 = Substitute.For<IMiddleware<MockUseCase, IMockOutputPort>>();
            _middleware2.Execute(
                    Arg.Any<MockUseCase>(),
                    Arg.Any<IMockOutputPort>(),
                    d => Task.FromResult(new UseCaseResult(true)),
                    Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(x => new UseCaseResult(true))
                .AndDoes(x => x.Arg<Func<MockUseCase, Task<UseCaseResult>>>().Invoke(x.Arg<MockUseCase>()));

            _globalMiddleware = Substitute.For<IMiddleware>();
            _globalMiddleware.Execute(
                    Arg.Any<MockUseCase>(),
                    d => Task.FromResult(new UseCaseResult(true)),
                    Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(x => new UseCaseResult(true))
                .AndDoes(x => x.Arg<Func<MockUseCase, Task<UseCaseResult>>>().Invoke(x.Arg<MockUseCase>()));


            builder.RegisterInstance(_useCaseInteractor).As<IInteractor<MockUseCase, IMockOutputPort>>();

            builder.RegisterInstance(_middleware1)
                .As<IMiddleware<MockUseCase, IMockOutputPort>>();
            builder.RegisterInstance(_middleware2)
                .As<IMiddleware<MockUseCase, IMockOutputPort>>();
            builder.RegisterInstance(_globalMiddleware)
                .As<IMiddleware>();

            _container = builder.Build();

            _interactorHub = new Hub(new AutoFacResolver(_container));
        }

        [Test]
        public async Task Test_AutoFac_Resolver()
        {
            await _interactorHub.Execute(new MockUseCase(), (IMockOutputPort)new MockOutputPort());
            await _useCaseInteractor.Received().Execute(Arg.Any<MockUseCase>(), Arg.Any<IMockOutputPort>(), Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task Test_Pipeline()
        {
            await _interactorHub.Execute(new MockUseCase(), (IMockOutputPort)new MockOutputPort());
            await _middleware2.ReceivedWithAnyArgs().Execute(Arg.Any<MockUseCase>(), Arg.Any<IMockOutputPort>(), Arg.Any<Func<MockUseCase, Task<UseCaseResult>>>(),
                Arg.Any<CancellationToken>());
        }
    }
}
