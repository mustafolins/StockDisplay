﻿using System;
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
        public double MovingAverageTen { get; set; }
        public double MovingAverageThirty { get; set; }
        public double StdDev10 { get; set; }
        public double StdDev30 { get; set; }
        public double BBUpper10 { get; set; }
        public double BBLower10 { get; set; }
        public double BBUpper30 { get; set; }
        public double BBLower30 { get; set; }

        public StockPoint SetDate(DateTime dateTime)
        {
            Date = dateTime;
            return this;
        }
    }

}
