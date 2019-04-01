using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpLearning.CrossValidation.TrainingTestSplitters;
using SharpLearning.InputOutput.Csv;
using SharpLearning.Metrics.Regression;
using SharpLearning.RandomForest.Learners;
using SharpLearning.RandomForest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace StockDisplay
{
    public partial class StockView : Form
    {
        public static List<StockPoint> StockPoints { get; set; }

        public StockView()
        {
            InitializeComponent();
        }

        private async void Go_ClickAsync(object sender, EventArgs e)
        {
            CurrentProgress.Show();
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }

            GetStockPoints();

            // add data points to chart 
            UpdateChart(StockPoints);

            var firstResult = await Task.Run<double>(() => 
            {
                return RetrieveStockDataAndTrainAI((PredictionLabel, TommorowPredictionLabel), Convert.ToInt32(Pattern1LengthUpDown.Value));
            });
            var secondResult = await Task.Run<double>(() => 
            {
                return RetrieveStockDataAndTrainAI((PredictionLabel2, TomorrowsPredictionLabel2), Convert.ToInt32(Pattern2LenthUpDown.Value));
            });
            var thirdResult = await Task.Run<double>(() => 
            {
                return RetrieveStockDataAndTrainAI((PredictionLabel3, TomorrowsPredictionLabel3), Convert.ToInt32(Pattern3LengthUpDown.Value));
            });

            var average = (firstResult + secondResult + thirdResult) / 3;

            AveragePredictionLabel.Text = $"Average Percent: {Math.Round(average, 5)}";
            ExpectedChangeLabel.Text = $"Expected Close: {Math.Round(average * double.Parse(StockPoints.Last().Close), 3)}";
        }

        private double RetrieveStockDataAndTrainAI((Label, Label) labels, int size)
        {
            // todo: add some more support for the alphavantage api

            if (StockPoints.Count() == 0)
            {
                MessageBox.Show($"Stock data not found for {SymbolToLoad.Text}",
                    "No Stock Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0.0;
            }

            // save csv and train ai and predict next closing price
            return SharpLearningUtility.PredictNextDataPoint(
                CsvUtilities.CreateTrainingDataFile(StockPoints, this, size), (labels.Item1, labels.Item2));
        }

        private void GetStockPoints()
        {
            // make request
            string responseString = GetStockData();

            // parse response
            var responseJobj = JObject.Parse(responseString);
            // get the points and sort them.
            StockPoints = GetDataPoints(responseJobj).OrderBy(sp => sp.Date).ToList();
        }

        private void UpdateChart(List<StockPoint> dataPoints)
        {
            AddDataPointsToCandlestickSeries(dataPoints);

            // moving average
            MovingAverage(dataPoints, 10);
            MovingAverage(dataPoints, 30, "MovingAverage30", 30);
        }

        private void MovingAverage(List<StockPoint> dataPoints, int numOfPoints, string series = "MovingAverage", int sizeOfAverage = 10)
        {
            for (int i = 0; i < dataPoints.Count; i++)
            {
                double average;
                if (i < sizeOfAverage)
                {
                    average = GetAverage(dataPoints.Take(i+1));
                }
                else
                {
                    average = GetAverage(dataPoints.Skip(i-sizeOfAverage).Take(sizeOfAverage));
                }
                chart1.Series[series].Points.AddXY(dataPoints[i].Date, average);
                if (sizeOfAverage == 10)
                {
                    StockPoints[i].MovingAverageTen = average; 
                }
                else if(sizeOfAverage == 30)
                {
                    StockPoints[i].MovingAverageThirty = average;
                }
            }
        }

        private double GetAverage(IEnumerable<StockPoint> dataPoints)
        {
            double total = 0.0;
            foreach (var point in dataPoints)
            {
                total += double.Parse(point.Close);
            }
            return total / dataPoints.Count();
        }

        private void AddDataPointsToCandlestickSeries(List<StockPoint> dataJPoints)
        {
            // add the stock points to the chart
            chart1.Series[0].Points.Clear();
            foreach (var point in dataJPoints)
            {
                chart1.Series[0].Points.AddXY(point.Date, double.Parse(point.High), double.Parse(point.Low),
                    double.Parse(point.Open), double.Parse(point.Close));
            }
            // change chart minimum area so it doesn't show all the way down to 0.0 on the Y axis
            chart1.ChartAreas[0].AxisY.Minimum = (from point in dataJPoints
                                                  orderby double.Parse(point.Low)
                                                  select double.Parse(point.Low) - 5).FirstOrDefault();
        }

        private string GetStockData()
        {
            var request = (HttpWebRequest)WebRequest.Create(
                            $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY" +
                            $"&symbol={SymbolToLoad.Text}" +
                            $"&outputsize={"compact"}&apikey=NBFOONK8Z8CG8J29"
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

        private void StockView_Load(object sender, EventArgs e)
        {
            // set up the chart series
            chart1.Series[0].XValueType = ChartValueType.Date;
            //chart1.Series[0].YValueMembers = "open,high,low,close";
            chart1.Series[0].CustomProperties = "PriceDownColor=Red,PriceUpColor=Blue";
            chart1.Series[0]["OpenCloseStyle"] = "Triangle";
            chart1.Series[0]["ShowOpenClose"] = "Both";

            chart1.Series["MovingAverage"].XValueType = ChartValueType.Date;
            chart1.Series["MovingAverage30"].XValueType = ChartValueType.Date;
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            //FindSymbol(SearchTextBox.Text);
        }

        private void FindSymbol(string text)
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

        private void SearchButton_Click(object sender, EventArgs e)
        {
            FindSymbol(SearchTextBox.Text);
        }

        private void SearchResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            SymbolToLoad.Text = SearchResults.SelectedItem.ToString();
        }
    }

}
