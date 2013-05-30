using Autofac;
using Moq;

namespace RadioChronicle.WebApi.Tests.Unit
{
    public class Bootstrap
    {

        public static IContainer DiContainer()
        {
            return null;
/*
            var builder = new ContainerBuilder();

            builder.RegisterType<Mock<IRequestHelper>>().As<Mock<IRequestHelper>>();
            builder.RegisterType<OdsluchaneEuAdapter>().
                .As<IRemoteServiceStrategy>()
                .WithMetadata("Name", "ConcreteStrategy");
         
            builder.RegisterAdapter<IRemoteServiceStrategy, RadioChronicleRemoteService>(s => new RadioChronicleRemoteService(s));

*/

        }

    }
}
