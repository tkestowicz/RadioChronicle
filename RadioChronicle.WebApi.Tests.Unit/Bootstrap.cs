using Autofac;
using HtmlAgilityPack;
using Moq;
using RadioChronicle.WebApi.Logic.Infrastracture;
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
            builder.RegisterType<OdsluchaneEuResponseParser>().As<IResponseParser>();
            builder.RegisterType<OdsluchaneEuUrlRepository>().As<IUrlRepository>();
            builder.RegisterType<OdsluchaneEuRemoteRadioChronicleServiceAdapter>().As<IRemoteRadioChronicleService>();
            builder.RegisterType<OdsluchaneEURemoteServiceArgumentsValidator>().As<IRemoteServiceArgumentsValidator>();
            
            return builder.Build();
        }

    }
}
