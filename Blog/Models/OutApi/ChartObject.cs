using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankiru.Models.OutApi
{
    public class ChartObject
    {
        [JsonProperty(PropertyName = "subject_id")]
        public int SubjectId { get; set; }
        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }
        [JsonProperty(PropertyName = "close")]
        public decimal Close { get; set; }
        [JsonProperty(PropertyName = "high")]
        [JsonIgnore]
        public decimal High { get; set; }
        [JsonProperty(PropertyName = "low")]
        [JsonIgnore]
        public decimal Low { get; set; }
        [JsonProperty(PropertyName = "open")]
        [JsonIgnore]
        public decimal Open { get; set; }
        [JsonProperty(PropertyName = "volume")]
        [JsonIgnore]
        public decimal Volume { get; set; }
    }
}
