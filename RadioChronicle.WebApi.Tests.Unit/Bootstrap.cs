using Autofac;
using Moq;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.OdsluchaneEu;

namespace RadioChronicle.WebApi.Tests.Unit
{
    public class Bootstrap
    {

        public static IContainer DiContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Mock<IRequestHelper>>().As<Mock<IRequestHelper>>();
            builder.RegisterType<OdsluchaneEuDOMParser>().As<IDOMParser>();
            builder.RegisterType<OdsluchaneEuUrlRepository>().As<IUrlRepository>();
            builder.RegisterType<OdsluchaneEuRemoteRadioChronicleServiceAdapter>().As<IRemoteRadioChronicleService>();

            return builder.Build();
        }

    }
}
