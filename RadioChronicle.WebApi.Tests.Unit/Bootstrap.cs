using System.Collections.Generic;
using Autofac;
using HtmlAgilityPack;
using Moq;
using RadioChronicle.WebApi.Logic.Infrastracture;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;
using RadioChronicle.WebApi.Logic.OdsluchaneEu;
using RadioChronicle.WebApi.Logic.OdsluchaneEu.Parsers;

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
            builder.RegisterType<TrackCollectionParser>().As<ICollectionParser<Track>, ICollectionParser<TrackHistory>, ICollectionParser<KeyValuePair<RadioStation, Track>>>();
            builder.RegisterType<SelectorHelper>().As<ISelectorHelper<HtmlNode>, ISelectorHelper<HtmlDocument>>();
            
            return builder.Build();
        }

    }
}
