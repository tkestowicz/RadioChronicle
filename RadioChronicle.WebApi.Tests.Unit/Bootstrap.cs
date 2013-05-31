using Autofac;
using Moq;
using RadioChronicle.WebApi.Logic.Infrastracture;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.OdsluchaneEu;

namespace RadioChronicle.WebApi.Tests.Unit
{
    public class Bootstrap
    {
        public enum RemoteServiceStrategy
        {
            StrategyContainer,
            OdsluchaneEuStrategy
        }

        public static IContainer DiContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Mock<IRequestHelper>>().As<Mock<IRequestHelper>>();
            builder.RegisterType<OdsluchaneEuDOMParser>().As<IDOMParser>();
            builder.RegisterType<OdsluchaneEuUrlRepository>().As<IUrlRepository>();

            builder.RegisterType<OdsluchaneEuRemoteServiceAdapter>()
                .Keyed<IRemoteServiceStrategy>(RemoteServiceStrategy.OdsluchaneEuStrategy);
         
            builder.RegisterType<RadioChronicleRemoteService>().Keyed<IRemoteServiceStrategy>(RemoteServiceStrategy.StrategyContainer);

            return builder.Build();
        }

    }
}
