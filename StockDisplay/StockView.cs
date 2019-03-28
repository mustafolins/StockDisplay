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
            // todo: add some more support for the alphavantage api
            // make request
            string responseString = GetStockData();

            // parse response
            var responseJobj = JObject.Parse(responseString);
            // get the points and sort them.
            var dataJPoints = GetDataPoints(responseJobj).OrderBy(sp => sp.Date);

            if (dataJPoints.Count() == 0)
            {
                MessageBox.Show($"Stock data not found for {SymbolToLoad.Text}", 
                    "No Stock Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

            PredictNextDataPoint(CreateTrainingDataFile(dataJPoints));
        }

        private (string, IOrderedEnumerable<StockPoint>) CreateTrainingDataFile(IOrderedEnumerable<StockPoint> dataJPoints)
        {
            var fileName = "traingdatatemp.csv";
            using (var sw = new StreamWriter(fileName))
            {
                sw.WriteLine(GetCsvColumnHeader(SizeOfPattern));
                var trainablePoints = dataJPoints.ToArray();
                for (int i = 0; i < trainablePoints.Length - SizeOfPattern; i++)
                {
                    var point = trainablePoints[i];
                    sw.WriteLine(GetCsvDataRow(trainablePoints, i, SizeOfPattern));                                                             // fourth day close
                }
            }
            return (fileName,dataJPoints);
        }

        private string GetCsvColumnHeader(int size)
        {
            var result = "";
            for (int i = 0; i < size; i++)
            {
                result += $"open{i},high{i},low{i},close{i},volume{i},";
            }
            return result + "lastclose";
        }

        private string GetCsvDataRow(StockPoint[] trainablePoints, int i, int size = 3)
        {
            var result = "";
            for (int j = 0; j < size; j++)
            {
                result += $"{trainablePoints[i + j].Open},{trainablePoints[i + j].High},{trainablePoints[i + j].Low}," +
                    $"{trainablePoints[i + j].Close},{trainablePoints[i + j].Volume},";
            }
            result += $"{trainablePoints[i + 3].Close}";
            return result;
        }

        private void PredictNextDataPoint((string, IOrderedEnumerable<StockPoint>) trainingData)
        {
            // Setup the CsvParser
            var parser = new CsvParser(
                () => new StreamReader(trainingData.Item1), separator: ',');

            // the column name in the temp data set we want to model.
            var targetName = "lastclose";

            // read the "close" column, this is the targets for our learner. 
            var targets = parser.EnumerateRows(targetName)
                .ToF64Vector();

            // read the feature matrix, all columns except "quality",
            // this is the observations for our learner.
            var observations = parser.EnumerateRows(c => c != targetName)
                .ToF64Matrix();



            // 30 % of the data is used for the test set. 
            var splitter = new RandomTrainingTestIndexSplitter<double>(trainingPercentage: 0.7, seed: 24);

            var trainingTestSplit = splitter.SplitSet(observations, targets);
            var trainSet = trainingTestSplit.TrainingSet;
            var testSet = trainingTestSplit.TestSet;



            // Create the learner and learn the model.
            var learner = new RegressionRandomForestLearner(trees: 1000);
            var model = learner.Learn(trainSet.Observations, trainSet.Targets);

            // predict the training and test set.
            var trainPredictions = model.Predict(trainSet.Observations);
            var testPredictions = model.Predict(testSet.Observations);

            // create the metric
            var metric = new MeanSquaredErrorRegressionMetric();

            // measure the error on training and test set.
            var trainError = metric.Error(trainSet.Targets, trainPredictions);
            var testError = metric.Error(testSet.Targets, testPredictions);



            // the variable importance requires the featureNameToIndex
            // from the data set. This mapping describes the relation
            // from column name to index in the feature matrix.
            var featureNameToIndex = parser.EnumerateRows(c => c != targetName)
                .First().ColumnNameToIndex; // todo: may want to change this to the close column

            // Get the variable importance from the model.
            var importances = model.GetVariableImportance(featureNameToIndex);



            // default format is xml.
            model.Save(() => new StreamWriter(@"randomforest.xml"));


            // default format is xml.
            //var loadedModel = RegressionForestModel.Load(() => new StreamReader(@"randomforest.xml"));

            // information about the accuracy of the prediction for what happened today
            var traingPointsArray = trainingData.Item2.Take(trainingData.Item2.Count() - 1).ToArray();
            var prediction = model.Predict(
                GetLastPattern(traingPointsArray, SizeOfPattern));
            var actualPoint = trainingData.Item2.Last();
            PredictionLabel.Text = $"Today's Prediction: {prediction}" +
                $" Expected Change: {Math.Round(prediction - double.Parse(traingPointsArray[traingPointsArray.Length - 1].Close), 3)}" +
                $" Off by: {Math.Round(double.Parse(actualPoint.Close) - prediction, 3)}" +
                $" Actual: {actualPoint.Close}";

            // tommorows prediction
            var traingPointsArray2 = trainingData.Item2.ToArray();
            var prediction2 = model.Predict(
                GetLastPattern(traingPointsArray2, SizeOfPattern));

            TommorowPredictionLabel.Text = $"Prediction: {prediction2}" +
                $" Expected Change: {Math.Round(prediction2 - double.Parse(traingPointsArray2[traingPointsArray2.Length - 1].Close), 3)}";
        }

        public int SizeOfPattern { get; set; }

        private double[] GetLastPattern(StockPoint[] points, int size)
        {
            var observation = new double[size * 5];
            for (int i = 0; i < size; i++)
            {
                observation[i + (i + 0)] = double.Parse(points[points.Length + i - size].Open);
                observation[i + (i + 1)] = double.Parse(points[points.Length + i - size].High);
                observation[i + (i + 2)] = double.Parse(points[points.Length + i - size].Low);
                observation[i + (i + 3)] = double.Parse(points[points.Length + i - size].Close);
                observation[i + (i + 4)] = double.Parse(points[points.Length + i - size].Volume);
            }
            return observation;
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

            SizeOfPattern = 10;
        }
    }

}
