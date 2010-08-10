using System;
using System.Xml.Linq;

namespace FlightsNorway.Extensions
{
    public static class XmlExtensions
    {
        public static string ElementValueOrEmpty(this XElement element, string elementName)
        {
            var childElement = element.Element(elementName);
            return childElement == null ? string.Empty : childElement.Value;
        }

        public static string AttributeValueOrEmpty(this XElement element, string attributeName)
        {
            var attribute = element.Attribute(attributeName);
            return attribute == null ? string.Empty : attribute.Value;            
        }

        public static DateTime ElementAsDateTime(this XElement element, string elementName)
        {
            var childElement = element.Element(elementName);
            return childElement == null ? DateTime.MinValue : Convert.ToDateTime(childElement.Value);
        }

        public static DateTime AttributeAsDateTime(this XElement element, string attributeName)
        {
            var attribute = element.Attribute(attributeName);
            return attribute == null ? DateTime.MinValue : Convert.ToDateTime(attribute.Value);
        }
    }
}
