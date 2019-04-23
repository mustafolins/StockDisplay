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
        public StockView()
        {
            InitializeComponent();
        }

        public bool GetAccuracy { get { return GetAccuracyCheckBox.Checked; } set { } }

        private async void Go_ClickAsync(object sender, EventArgs e)
        {
            CurrentProgress.Show();
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }

            int size = int.Parse(ChartLength.SelectedItem.ToString());

            var spxPoints = GetStockPoints(true, size);
            CalculateTechnicalIndicators(spxPoints, true);
            LodingInfoLabel.Text = "Analyzing current market trends.";
            double spxAverage = await GetAveragePercent(spxPoints);
            SpxAverage.Text = $"SPX Percent: {Math.Round(spxAverage, 5)}";

            List<StockPoint> testSymbolPoints = new List<StockPoint>();
            var symbolPoints = GetStockPoints(false, 0, size);

            if (GetAccuracy)
            {
                var accuaracyTestSize = int.Parse(ChartLength.SelectedItem.ToString()) / int.Parse(AccuracyTestSize.SelectedItem.ToString());
                double first = 0.0, second = 0.0, last = 0.0;

                for (int i = 1; i < accuaracyTestSize; i++)
                {
                    do
                    {
                        testSymbolPoints = GetStockPoints(false, i, size);
                    } while (testSymbolPoints.Count != size);

                    CalculateTechnicalIndicators(testSymbolPoints, true);

                    var (firstPred, secondPred, lastPred) = await GetPredictions(testSymbolPoints);
                    var nextPoint = symbolPoints.FirstOrDefault(sp => sp.Date == testSymbolPoints.Last().Date.AddDays(1));
                    if (nextPoint == null) // is friday/holiday? so get next available day
                    {
                        nextPoint = symbolPoints.FirstOrDefault(sp => sp.Date >= testSymbolPoints.Last().Date.AddDays(1));
                    }
                    first += (double.Parse(testSymbolPoints.Last().Close) * firstPred) / (double.Parse(nextPoint.Close));
                    second += (double.Parse(testSymbolPoints.Last().Close) * secondPred) / (double.Parse(nextPoint.Close));
                    last += (double.Parse(testSymbolPoints.Last().Close) * lastPred) / (double.Parse(nextPoint.Close));
                }

                double fAverage = first / accuaracyTestSize;
                double sAverage = second / accuaracyTestSize;
                double lAverage = last / accuaracyTestSize;

                AccuracyLabel.Text = $"Accuracy: P1 - {Math.Round(fAverage, 5)} P2 - {Math.Round(sAverage, 5)} P3 - {Math.Round(lAverage, 5)}";
            }

            // add data points to chart 
            CalculateTechnicalIndicators(symbolPoints);
            LodingInfoLabel.Text = $"Analyzing trends for {SymbolToLoad.Text.ToUpper()}.";
            double average = await GetAveragePercent(symbolPoints);

            AveragePredictionLabel.Text = $"Average Percent: {Math.Round(average, 5)}";
            if (symbolPoints.Count > 0)
            {
                ExpectedChangeLabel.Text = $"Expected Close: {Math.Round(average * double.Parse(symbolPoints.Last().Close), 3)}";
                InclusiveAverage.Text = $"Average (Including SPX): {Math.Round((average + spxAverage) / 2.0, 5)}";
            }
            LodingInfoLabel.Text = "Done";
            CurrentProgress.Hide();

            await Task.Delay(3000);
            LodingInfoLabel.Text = "";
        }

        private async Task<double> GetAveragePercent(List<StockPoint> stockPoints)
        {
            var (first, second, third) = await GetPredictions(stockPoints);

            var average = (first + second + third) / 3;
            return average;
        }

        private async Task<(double, double, double)> GetPredictions(List<StockPoint> stockPoints)
        {
            var firstResult = await Task.Run<double>(() =>
            {
                return RetrieveStockDataAndTrainAI(stockPoints, (PredictionLabel, TommorowPredictionLabel), Convert.ToInt32(Pattern1LengthUpDown.Value));
            });
            var secondResult = await Task.Run<double>(() =>
            {
                return RetrieveStockDataAndTrainAI(stockPoints, (PredictionLabel2, TomorrowsPredictionLabel2), Convert.ToInt32(Pattern2LenthUpDown.Value));
            });
            var thirdResult = await Task.Run<double>(() =>
            {
                return RetrieveStockDataAndTrainAI(stockPoints, (PredictionLabel3, TomorrowsPredictionLabel3), Convert.ToInt32(Pattern3LengthUpDown.Value));
            });
            return (firstResult, secondResult, thirdResult);
        }

        private double RetrieveStockDataAndTrainAI(List<StockPoint> stockPoints, (Label, Label) labels, int size)
        {
            // todo: add some more support for the alphavantage api

            if (stockPoints.Count() == 0)
            {
                MessageBox.Show($"Stock data not found for {SymbolToLoad.Text}",
                    "No Stock Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0.0;
            }

            // save csv and train ai and predict next closing price
            return SharpLearningUtility.PredictNextDataPoint(
                CsvUtilities.CreateTrainingDataFile(stockPoints, this, size), (labels.Item1, labels.Item2));
        }

        private List<StockPoint> GetStockPoints(bool spx = false, int offset = 0, int size = 90)
        {
            // make request
            string responseString = GetStockData(spx);

            // parse response
            var responseJobj = JObject.Parse(responseString);
            // get the points and sort them.
            var tempPoints = GetDataPoints(responseJobj);
            var temp = tempPoints.OrderBy(sp => sp.Date).Skip(tempPoints.Count() - (size + offset)).Take(size).ToList();
            return temp;
        }

        private void CalculateTechnicalIndicators(List<StockPoint> dataPoints, bool isSpx = false)
        {
            if (!isSpx)
                AddDataPointsToCandlestickSeries(dataPoints);

            // moving average
            MovingAverage(dataPoints, "MovingAverage", 10, isSpx);
            MovingAverage(dataPoints, "MovingAverage30", 30, isSpx);
            StandardDeviation(dataPoints, 10);
            StandardDeviation(dataPoints, 30);
            BollingerBand(dataPoints, 10);
            BollingerBand(dataPoints, 30);
        }

        private void BollingerBand(List<StockPoint> dataPoints, int sizeOfAverage)
        {
            for (int i = 0; i < dataPoints.Count; i++)
            {
                double stdDev;
                double average;
                if (i < sizeOfAverage)
                {
                    stdDev = GetStdDev(dataPoints.Take(i + 1));
                    average = GetAverage(dataPoints.Take(i + 1));
                }
                else
                {
                    stdDev = GetStdDev(dataPoints.Skip(i - sizeOfAverage).Take(sizeOfAverage));
                    average = GetAverage(dataPoints.Skip(i - sizeOfAverage).Take(sizeOfAverage));
                }

                if (sizeOfAverage == 10)
                {
                    dataPoints[i].BBLower10 = stdDev * 2 - average;
                    dataPoints[i].BBUpper10 = stdDev * 2 + average;
                }
                else if (sizeOfAverage == 30)
                {
                    dataPoints[i].BBLower30 = stdDev * 2 - average;
                    dataPoints[i].BBUpper30 = stdDev * 2 + average;
                }
            }
        }

        private void StandardDeviation(List<StockPoint> dataPoints, int sizeOfAverage)
        {
            for (int i = 0; i < dataPoints.Count; i++)
            {
                double stdDeviation;
                if (i < sizeOfAverage)
                {
                    stdDeviation = GetStdDev(dataPoints.Take(i + 1));
                }
                else
                {
                    stdDeviation = GetStdDev(dataPoints.Skip(i - sizeOfAverage).Take(sizeOfAverage));
                }

                if (sizeOfAverage == 10)
                {
                    dataPoints[i].StdDev10 = stdDeviation;
                }
                else if (sizeOfAverage == 30)
                {
                    dataPoints[i].StdDev30 = stdDeviation;
                }
            }
        }

        private double GetStdDev(IEnumerable<StockPoint> dataPoints)
        {
            var mean = GetAverage(dataPoints);
            return Math.Sqrt(
                dataPoints.Sum(
                    x => Math.Pow(double.Parse(x.Close) - mean, 2)) / dataPoints.Count());
        }

        private void MovingAverage(List<StockPoint> dataPoints, string series = "MovingAverage", int sizeOfAverage = 10, bool isSpx = false)
        {
            for (int i = 0; i < dataPoints.Count; i++)
            {
                double average;
                if (i < sizeOfAverage)
                {
                    average = GetAverage(dataPoints.Take(i + 1));
                }
                else
                {
                    average = GetAverage(dataPoints.Skip(i - sizeOfAverage).Take(sizeOfAverage));
                }
                if (!isSpx)
                    chart1.Series[series].Points.AddXY(dataPoints[i].Date, average);
                if (sizeOfAverage == 10)
                {
                    dataPoints[i].MovingAverageTen = average;
                }
                else if (sizeOfAverage == 30)
                {
                    dataPoints[i].MovingAverageThirty = average;
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

        private string GetStockData(bool spx = false)
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

            ChartLength.SelectedItem = ChartLength.Items[1];
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

        private void GetAccuracyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            AccuracyTestSize.SelectedIndex = 0;
        }
    }

}
