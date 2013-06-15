using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.SpecifiedDOMParsers
{
    public class OdsluchaneEuRadioStationParser : ISpecifiedDOMParser<RadioStation, IEnumerable<HtmlNode>>
    {
        private readonly RadioStation _parsedRadioStation = new RadioStation();
        private const int _IndexOfRadioStationElementInCurrentlyBroadcastedList = 0;
        private const int _IndexOfRadioStationElementInTrackHistoryList = 1;

        #region Implementation of ISpecifiedDOMParser<out RadioStation,in IEnumerable<HtmlNode>>

        public RadioStation Parse(IEnumerable<HtmlNode> input)
        {
            try
            {
                var index = _DetermineCallContext(input);

                // this check reduces number of thrown exceptions
                if (index >= 0)
                    _ParseName(input.ElementAt(index));
            }
            catch
            {
            }

            return _parsedRadioStation;
        }

        public RadioStation Parse(IEnumerable<HtmlAttribute> input)
        {
            try
            {
                _ParseId(input);
                _ParseName(input);
                _ParseIsDefault(input);
            }
            catch
            {
            }

            return _parsedRadioStation;
        }

        #endregion

        private int _DetermineCallContext(IEnumerable<HtmlNode> htmlNodes)
        {
            const int numberOfCellsInTrackHistoryRow = 2;
            const int numberOfCellsInCurrentlyBroadcastedRow = 3;

            Func<IEnumerable<HtmlNode>, bool> contextIsTrackHistory =
                nodes => nodes.Count() == numberOfCellsInTrackHistoryRow;

            Func<IEnumerable<HtmlNode>, bool> contextIsCurrentlyBroadcasted =
                nodes => nodes.Count() == numberOfCellsInCurrentlyBroadcastedRow && nodes.FirstOrDefault().NodeType == HtmlNodeType.Text;

            if (contextIsCurrentlyBroadcasted(htmlNodes))
                return _IndexOfRadioStationElementInCurrentlyBroadcastedList;

            if (contextIsTrackHistory(htmlNodes)) return _IndexOfRadioStationElementInTrackHistoryList;

            const int undefinedContext = -1;

            return undefinedContext;
        }

        private void _ParseId(IEnumerable<HtmlAttribute> attributes)
        {
            var radioId = attributes.SingleOrDefault(a => a.Name == "value");

            if (radioId != null) _parsedRadioStation.Id = int.Parse(radioId.Value);
        }

        private void _ParseIsDefault(IEnumerable<HtmlAttribute> attributes)
        {
            var isSelected = attributes.SingleOrDefault(a => a.Name == "selected");

            _parsedRadioStation.IsDefault = isSelected != null && isSelected.Value == "selected";
        }

        private void _ParseName(IEnumerable<HtmlAttribute> attributes)
        {
            var radioName = attributes.SingleOrDefault(a => a.Name == "label");

            if(radioName != null) _parsedRadioStation.Name = radioName.Value;
        }

        private void _ParseName(HtmlNode cell)
        {
            _parsedRadioStation.Name = cell.InnerText.Trim();
        }
    }
}