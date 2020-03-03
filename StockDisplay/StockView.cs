using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public bool CalculateAccuracy { get { return GetAccuracyCheckBox.Checked; } set { } }
        public Fetcher Fetch { get; set; }
        public SharpLearningUtility LearningUtility { get; set; }
        public TechnicalIndicators Indicators { get; set; }

        private async void Go_ClickAsync(object sender, EventArgs e)
        {
            // show progress bar
            CurrentProgress.Show();
            // clear chart series
            foreach (var series in StockChart.Series)
            {
                series.Points.Clear();
            }

            // get total chart length to retrieve
            int size = int.Parse(ChartLength.SelectedItem.ToString());

            // gets the s&p 500 data points
            var spxPoints = Fetch.GetStockPoints(true, 0, size);
            CalculateTechnicalIndicators(spxPoints, true);
            LodingInfoLabel.Text = "Analyzing current market trends.";
            double spxAverage = await GetAveragePercent(spxPoints);
            SpxAverage.Text = $"SPX Percent: {Math.Round(spxAverage, 5)}";

            var symbolPoints = Fetch.GetStockPoints(false, 0, size);

            // calculate the accuaracy of the current predictions 
            if (CalculateAccuracy)
            {
                await GetAccuracy(size, symbolPoints);
            }

            // add data points to chart 
            CalculateTechnicalIndicators(symbolPoints);
            LodingInfoLabel.Text = $"Analyzing trends for {SymbolToLoad.Text.ToUpper()}.";
            // get the average prediction of the three pattern sizes
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

        /// <summary>
        /// Gets the accuracy for the current selected pattern sizes.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="testSymbolPoints"></param>
        /// <param name="symbolPoints"></param>
        /// <returns></returns>
        private async Task GetAccuracy(int size, List<StockPoint> symbolPoints)
        {
            List<StockPoint> testSymbolPoints = new List<StockPoint>();

            // get accuracy test size from accuracy combo box
            var accuaracyTestSize = int.Parse(ChartLength.SelectedItem.ToString()) / int.Parse(AccuracyTestSize.SelectedItem.ToString());
            double first = 0.0, second = 0.0, last = 0.0;

            for (int i = 1; i < accuaracyTestSize; i++)
            {
                // get sub set of stock data points from full data set symbol points (excluding day being predicted)
                testSymbolPoints = Fetch.GetSubStockPoints(i, size - i, symbolPoints);

                // calculate the technical indicators
                CalculateTechnicalIndicators(testSymbolPoints, true);

                // get the predictions for the test data points
                var (firstPred, secondPred, lastPred) = await GetPredictions(testSymbolPoints);

                // get the actual data point for the day being predicted
                var nextPoint = symbolPoints.FirstOrDefault(sp => sp.Date == testSymbolPoints.Last().Date.AddDays(1));
                if (nextPoint == null) // is friday/holiday? so get next available day
                {
                    nextPoint = symbolPoints.FirstOrDefault(sp => sp.Date >= testSymbolPoints.Last().Date.AddDays(1));
                }

                // plot p1 guesses
                if (P1CheckBox.Checked)
                {
                    StockChart.Series["P1Guesses"].Points.AddXY(nextPoint.Date, firstPred * double.Parse(testSymbolPoints.Last().Close));
                }
                // plot p2 guesses
                if (P2Checkbox.Checked)
                {
                    StockChart.Series["P2Guesses"].Points.AddXY(nextPoint.Date, secondPred * double.Parse(testSymbolPoints.Last().Close));
                }
                // plot p3 guesses
                if (P3Checkbox.Checked)
                {
                    StockChart.Series["P3Guesses"].Points.AddXY(nextPoint.Date, lastPred * double.Parse(testSymbolPoints.Last().Close));
                }

                // add test accuracy to the current total
                first += Math.Abs(firstPred - (double.Parse(testSymbolPoints.Last().Close) / double.Parse(nextPoint.Close)));
                second += Math.Abs(secondPred - (double.Parse(testSymbolPoints.Last().Close) / double.Parse(nextPoint.Close)));
                last += Math.Abs(lastPred - (double.Parse(testSymbolPoints.Last().Close) / double.Parse(nextPoint.Close)));
            }

            // calculate the averages for the test predictions
            double fAverage = 1.0 - (first / accuaracyTestSize);
            double sAverage = 1.0 - (second / accuaracyTestSize);
            double lAverage = 1.0 - (last / accuaracyTestSize);

            // display results in accuracy label
            AccuracyLabel.Text = $"Accuracy: P1 - {Math.Round(fAverage, 5)} P2 - {Math.Round(sAverage, 5)} P3 - {Math.Round(lAverage, 5)}";
        }

        /// <summary>
        /// Gets the average percent change prediction for the three selected pattern lengths.
        /// </summary>
        /// <param name="stockPoints">The stock points to get predictions for and average.</param>
        /// <returns>The average predicted percent change.</returns>
        private async Task<double> GetAveragePercent(List<StockPoint> stockPoints)
        {
            var (first, second, third) = await GetPredictions(stockPoints);

            var average = (first + second + third) / 3;
            return average;
        }

        private async Task<(double, double, double)> GetPredictions(List<StockPoint> stockPoints)
        {
            // get predictions using first pattern size
            var firstResult = await Task.Run<double>(() =>
            {
                return RetrieveStockDataAndTrainAI(stockPoints, (PredictionLabel, TommorowPredictionLabel), Convert.ToInt32(Pattern1LengthUpDown.Value));
            });
            // get predictions using second pattern size
            var secondResult = await Task.Run<double>(() =>
            {
                return RetrieveStockDataAndTrainAI(stockPoints, (PredictionLabel2, TomorrowsPredictionLabel2), Convert.ToInt32(Pattern2LenthUpDown.Value));
            });
            // get predictions using third pattern size
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
            return LearningUtility.PredictNextDataPoint(
                CsvUtilities.CreateTrainingDataFile(stockPoints, this, size), (labels.Item1, labels.Item2));
        }

        private void CalculateTechnicalIndicators(List<StockPoint> dataPoints, bool isSpx = false)
        {
            if (!isSpx)
                AddDataPointsToCandlestickSeries(dataPoints);

            // moving average
            Indicators.MovingAverage(dataPoints, "MovingAverage", 10, isSpx);
            Indicators.MovingAverage(dataPoints, "MovingAverage30", 30, isSpx);
            Indicators.StandardDeviation(dataPoints, 10);
            Indicators.StandardDeviation(dataPoints, 30);
            Indicators.BollingerBand(dataPoints, 10, isSpx);
            Indicators.BollingerBand(dataPoints, 30, isSpx);
        }

        private void AddDataPointsToCandlestickSeries(List<StockPoint> dataJPoints)
        {
            // add the stock points to the chart
            StockChart.Series[0].Points.Clear();
            foreach (var point in dataJPoints)
            {
                StockChart.Series[0].Points.AddXY(point.Date, double.Parse(point.High), double.Parse(point.Low),
                    double.Parse(point.Open), double.Parse(point.Close));
            }
            // change chart minimum area so it doesn't show all the way down to 0.0 on the Y axis
            StockChart.ChartAreas[0].AxisY.Minimum = (from point in dataJPoints
                                                  orderby double.Parse(point.Low)
                                                  select double.Parse(point.Low) - 5).FirstOrDefault();
        }

        private void StockView_Load(object sender, EventArgs e)
        {
            Fetch = new Fetcher(SymbolToLoad, SearchTextBox, SearchResults);
            LearningUtility = new SharpLearningUtility();

            // set up the chart series
            StockChart.Series[0].XValueType = ChartValueType.Date;
            //chart1.Series[0].YValueMembers = "open,high,low,close";
            StockChart.Series[0].CustomProperties = "PriceDownColor=Red,PriceUpColor=Blue";
            StockChart.Series[0]["OpenCloseStyle"] = "Triangle";
            StockChart.Series[0]["ShowOpenClose"] = "Both";

            StockChart.Series["MovingAverage"].XValueType = ChartValueType.Date;
            StockChart.Series["MovingAverage30"].XValueType = ChartValueType.Date;
            StockChart.Series["BBUpper10"].XValueType = ChartValueType.Date;
            StockChart.Series["BBLower10"].XValueType = ChartValueType.Date;
            StockChart.Series["BBUpper30"].XValueType = ChartValueType.Date;
            StockChart.Series["BBLower30"].XValueType = ChartValueType.Date;

            ChartLength.SelectedItem = ChartLength.Items[1];

            Indicators = new TechnicalIndicators(StockChart);
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            //FindSymbol(SearchTextBox.Text);
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            // search for the requested symbol
            Fetch.FindSymbol(SearchTextBox.Text);
        }

        private void SearchResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            SymbolToLoad.Text = SearchResults.SelectedItem.ToString();
        }

        private void GetAccuracyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // select first accuracy option automatically on check
            AccuracyTestSize.SelectedIndex = 0;
        }

        private void BBCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // change chart series to reflect desired choice
            StockChart.Series["BBLower10"].Enabled = BBCheckBox.Checked;
            StockChart.Series["BBUpper10"].Enabled = BBCheckBox.Checked;
            StockChart.Series["BBLower30"].Enabled = BBCheckBox.Checked;
            StockChart.Series["BBUpper30"].Enabled = BBCheckBox.Checked;
        }

        private void AveragesCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            // change chart series to reflect desired choice
            StockChart.Series["MovingAverage"].Enabled = AveragesCheckbox.Checked;
            StockChart.Series["MovingAverage30"].Enabled = AveragesCheckbox.Checked;
        }

        private void P1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            StockChart.Series["P1Guesses"].Enabled = P1CheckBox.Checked;
        }

        private void P2Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            StockChart.Series["P2Guesses"].Enabled = P2Checkbox.Checked;
        }

        private void P3Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            StockChart.Series["P3Guesses"].Enabled = P3Checkbox.Checked;
        }
    }

}
