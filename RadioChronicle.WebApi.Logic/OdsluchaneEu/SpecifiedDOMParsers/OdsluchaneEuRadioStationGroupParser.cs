using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.SpecifiedDOMParsers
{
    public class OdsluchaneEuRadioStationGroupParser : ISpecifiedDOMParser<RadioStationGroup, HtmlNode>
    {
        private const int _IndexOfRadioGroupNameElement = 0;

        private readonly IDOMSelector _domSelector;

        private RadioStationGroup _parsedRadioStationGroup;
        private readonly ISpecifiedDOMParser<RadioStation, IEnumerable<HtmlAttribute>> _radioParser;

        public OdsluchaneEuRadioStationGroupParser(IDOMSelector domSelector, ISpecifiedDOMParser<RadioStation, IEnumerable<HtmlAttribute>> radioParser)
        {
            _domSelector = domSelector;
            _radioParser = radioParser;
        }

        #region Implementation of ISpecifiedDOMParser<out RadioStationGroup,in IEnumerable<HtmlNode>>

        public RadioStationGroup Parse(HtmlNode input)
        {
            _parsedRadioStationGroup = RadioStationGroup.Empty;

            try
            {
                _ParseGroupName(input.Attributes.ElementAt(_IndexOfRadioGroupNameElement));
                _ParseRadioStations(_domSelector.SelectRadioStations(input));
            }
            catch
            {
            }

            return _parsedRadioStationGroup;
        }

        #endregion

        private void _ParseRadioStations(IEnumerable<HtmlNode> rows)
        {
            var result = new List<RadioStation>();

            if (rows != null)
            {
                foreach (var radioStation in rows)
                {
                    if (!radioStation.Attributes.Any()) continue;

                    result.Add(_radioParser.Parse(radioStation.Attributes));
                }
            }

            _parsedRadioStationGroup.RadioStations = result;
        }

        private void _ParseGroupName(HtmlAttribute attribute)
        {
            _parsedRadioStationGroup.Name = attribute.Value;
        }
    }
}