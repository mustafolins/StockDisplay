using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockDisplay
{
    public class Fetcher
    {
        public TextBox SymbolToLoad { get; set; }
        public TextBox SearchTextBox { get; set; }
        public ComboBox SearchResults { get; set; }

        public Fetcher(TextBox symbolToLoad, TextBox searchTextbox, ComboBox searchResults)
        {
            SymbolToLoad = symbolToLoad;
            SearchTextBox = searchTextbox;
            SearchResults = searchResults;
        }

        public List<StockPoint> GetStockPoints(bool spx = false, int offset = 0, int size = 90)
        {
            // make request
            string responseString = GetStockData(spx);

            // parse response
            var responseJobj = JObject.Parse(responseString);
            // get the points and sort them.
            var tempPoints = GetDataPoints(responseJobj);
            List<StockPoint> temp = GetSubStockPoints(offset, size, tempPoints);
            return temp;
        }

        /// <summary>
        /// Gets the subset of the given <paramref name="tempPoints"/>.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="tempPoints"></param>
        /// <returns></returns>
        public List<StockPoint> GetSubStockPoints(int offset, int size, IEnumerable<StockPoint> tempPoints)
        {
            return tempPoints.OrderBy(sp => sp.Date).Skip(tempPoints.Count() - (size + offset)).Take(size).ToList();
        }

        public string GetStockData(bool spx = false)
        {
            var request = (HttpWebRequest)WebRequest.Create(
                            $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY" +
                            $"&symbol={((spx) ? "SPX" : SymbolToLoad.Text)}" +
                            $"&outputsize={"full"}&apikey=NBFOONK8Z8CG8J29"
                            );
            // process response
            var response = request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            response.Close();
            return responseString;
        }

        /// <summary>
        /// Converts the JSON to StockPoints.
        /// </summary>
        /// <param name="responseJobj"></param>
        /// <returns></returns>
        private IEnumerable<StockPoint> GetDataPoints(JObject responseJobj)
        {
            foreach (var point in responseJobj.Last.First)
            {
                yield return (JsonConvert.DeserializeObject<StockPoint>(point.First.ToString()))
                    .SetDate(DateTime.Parse(((JProperty)point).Name));
            }
        }

        public void FindSymbol(string text)
        {
            var response = QuerySymbols();
            var responseJObject = JObject.Parse(response);
            if (responseJObject["Error Message"] != null) return;

            SearchResults.Items.Clear();
            foreach (var symbolData in responseJObject["bestMatches"])
            {
                var sym = symbolData["1. symbol"].ToString();
                SearchResults.Items.Add(sym);
            }
            if (SearchResults.Items.Count > 0)
                SearchResults.SelectedItem = SearchResults.Items[0];
        }

        private string QuerySymbols()
        {
            var request = (HttpWebRequest)WebRequest.Create(
                                        $"https://www.alphavantage.co/query?function=SYMBOL_SEARCH" +
                                        $"&keywords={SearchTextBox.Text}" +
                                        $"&apikey=NBFOONK8Z8CG8J29"
                                        );
            // process response
            var response = request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            response.Close();
            return responseString;
        }
    }
}
