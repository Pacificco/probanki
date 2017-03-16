using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bankiru.Models.OutApi.CBR
{
    /// <summary>
    /// Класс, представляющий XML-ответ от сайта Центробанка России
    /// </summary>
    [XmlRoot("ValCurs")]
    public class XML_CBRAnswer
    {
        [XmlAttribute]
        public string Date;

        [XmlAttribute]
        public string name;

        [XmlElementAttribute("Record")]
        public XML_CurrencyItem[] CurrencyItems;
    }

    /// <summary>
    /// Класс, представляющий валюту в XML-ответе с сайта
    /// </summary>
    public class XML_CurrencyItem
    {
        [XmlAttribute]
        public string Id;

        [XmlAttribute]
        public string Date;

        [XmlElementAttribute("Nominal")]
        public int Nominal;

        [XmlElementAttribute("Value")]
        public string Value;
    }
}
