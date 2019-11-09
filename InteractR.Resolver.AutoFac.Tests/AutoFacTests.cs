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


        [SetUp]
        public void Setup()
        {
            var builder = new ContainerBuilder();
            _useCaseInteractor = Substitute.For<IInteractor<MockUseCase, IMockOutputPort>>();

            builder.RegisterInstance(_useCaseInteractor).As<IInteractor<MockUseCase, IMockOutputPort>>();

            _container = builder.Build();

            _interactorHub = new Hub(new AutoFacResolver(_container));
        }

        [Test]
        public async Task Test_AutoFac_Resolver()
        {
            await _interactorHub.Execute(new MockUseCase(), (IMockOutputPort)new MockOutputPort());
            await _useCaseInteractor.Received().Execute(Arg.Any<MockUseCase>(), Arg.Any<IMockOutputPort>(), Arg.Any<CancellationToken>());
        }

    }
}
