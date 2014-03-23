using Autofac;
using Moq;
using RadioChronicle.WebApi.Logic.Infrastracture;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.OdsluchaneEu;
using RadioChronicle.WebApi.Logic.OdsluchaneEu.Helpers;
using RadioChronicle.WebApi.Logic.OdsluchaneEu.Interfaces;
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
            builder.RegisterType<OdsluchaneEuSelectorsRepository>().As<IXPathSelectorsRepository>();
            builder.RegisterType<OdsluchaneEuServiceAdapter>().As<IRemoteRadioChronicleService>();
            builder.RegisterType<OdsluchaneEURemoteServiceArgumentsValidator>().As<IRemoteServiceArgumentsValidator>();
            builder.RegisterType<TrackCollectionParser>().As<ITrackCollectionParser>();
            builder.RegisterType<HtmlDocumentHelper>().As<IHtmlDocumentHelper>();
            builder.RegisterType<MostPopularTrackParser>().As<IMostPopularTrackParser>();
            builder.RegisterType<NewestTrackParser>().As<INewestTrackParser>();
            builder.RegisterType<CurrentlyBroadcastedTrackParser>().As<ICurrentlyBroadcastedTrack>();
            builder.RegisterType<TrackBroadcastHistoryParser>().As<ITrackBroadcastHistoryParser>();
            builder.RegisterType<TrackHistoryParser>().As<ITrackHistoryParser>();
            builder.RegisterType<RadioStationGroupParser>().As<IRadioGroupParser>();
            builder.RegisterType<RadioStationParser>().As<IRadioStationParser>();
            builder.RegisterType<RadioStationCollectionParser>().As<IRadioStationCollectionParser>();
            builder.RegisterType<OdsluchaneEuOdsluchaneEuResponseHelper>().As<IOdsluchaneEuResponseHelper>();
            
            return builder.Build();
        }

    }
}
