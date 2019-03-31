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

        private void Go_Click(object sender, EventArgs e)
        {
            CurrentProgress.Show();
            Task.Run(() => { RetrieveStockDataAndTrainAI(); });
        }

        private void RetrieveStockDataAndTrainAI()
        {
            // todo: add some more support for the alphavantage api
            // make request
            string responseString = GetStockData();

            // parse response
            var responseJobj = JObject.Parse(responseString);
            // get the points and sort them.
            var dataPoints = GetDataPoints(responseJobj).OrderBy(sp => sp.Date);

            if (dataPoints.Count() == 0)
            {
                MessageBox.Show($"Stock data not found for {SymbolToLoad.Text}",
                    "No Stock Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // add data points to chart 
            if (chart1.InvokeRequired)
            {
                Invoke(new MethodInvoker(
                    delegate ()
                    {
                        UpdateChart(dataPoints);
                    }));
            }
            else
            {
                UpdateChart(dataPoints);
            }
            
            // save csv and train ai and predict next closing price
            SharpLearningUtility.PredictNextDataPoint(
                CsvUtilities.CreateTrainingDataFile(dataPoints, this, 10), (PredictionLabel, TommorowPredictionLabel));
        }

        private void UpdateChart(IOrderedEnumerable<StockPoint> dataPoints)
        {
            AddDataPointsToCandlestickSeries(dataPoints);

            // moving average
            MovingAverage(dataPoints.ToList(), 10);
            MovingAverage(dataPoints.ToList(), 30, "MovingAverage30", 30);
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

        private void AddDataPointsToCandlestickSeries(IOrderedEnumerable<StockPoint> dataJPoints)
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
    }

}
