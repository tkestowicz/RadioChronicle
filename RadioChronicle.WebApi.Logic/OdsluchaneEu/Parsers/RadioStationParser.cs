using System.Linq;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.Parsers
{
    public class RadioStationParser : IRowParser<RadioStation>
    {
        #region Implementation of IRowParser<out RadioStation>

        public HtmlNode GroupNode { set; private get; }

        public RadioStation Parse(HtmlNode node)
        {
            var radioName = node.Attributes.SingleOrDefault(a => a.Name == "label");
            var radioId = node.Attributes.SingleOrDefault(a => a.Name == "value");

            var isSelected = node.Attributes.SingleOrDefault(a => a.Name == "selected");
            var isDefault = isSelected != null && isSelected.Value == "selected";

            return new RadioStation()
            {
                Id = (radioId != null) ? int.Parse(radioId.Value) : 0,
                Name = (radioName != null) ? radioName.Value : "",
                IsDefault = isDefault
            };
        }

        #endregion
    }
}