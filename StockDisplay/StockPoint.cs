using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace StockDisplay
{
    public class StockPoint
    {
        [JsonProperty("1. open")]
        public string Open { get; set ; }
        [JsonProperty("2. high")]
        public string High { get; set; }
        [JsonProperty("3. low")]
        public string Low { get; set; }
        [JsonProperty("4. close")]
        public string Close { get; set; }
        [JsonProperty("5. volume")]
        public string Volume { get; set; }
        public DateTime Date { get; set; }

        public IEnumerable<double> GetValues()
        {
            yield return double.Parse(Open);
            yield return double.Parse(High);
            yield return double.Parse(Low);
            yield return double.Parse(Close);
            //yield return double.Parse(Volume);
        }

        public StockPoint SetDate(DateTime dateTime)
        {
            Date = dateTime;
            return this;
        }
    }

}
