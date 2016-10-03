using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Currencies
{
    /// <summary>
    /// Класс, представляющий XML-ответ от сайта Центробанка России
    /// </summary>
    [XmlRoot("ValCurs")]
    public class XML_Answer
    {
        [XmlAttribute]
        public string Date;

        [XmlAttribute]
        public string name;

        [XmlElementAttribute("Valute")]
        public XML_CurrencyItem[] CurrencyItems;
    }

    /// <summary>
    /// Класс, представляющий валюту в XML-ответе с сайта
    /// </summary>
    public class XML_CurrencyItem
    {
        [XmlElementAttribute("NumCode")]
        public int NumCode;

        [XmlElementAttribute("CharCode")]
        public string CharCode;

        [XmlElementAttribute("Nominal")]
        public int Nominal;

        [XmlElementAttribute("Name")]
        public string Name;

        [XmlElementAttribute("Value")]
        public string Value;
    }
}
