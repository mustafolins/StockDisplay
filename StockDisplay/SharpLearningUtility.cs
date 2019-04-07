using SharpLearning.Containers.Matrices;
using SharpLearning.CrossValidation.TrainingTestSplitters;
using SharpLearning.InputOutput.Csv;
using SharpLearning.Metrics.Regression;
using SharpLearning.RandomForest.Learners;
using SharpLearning.RandomForest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockDisplay
{
    public static class SharpLearningUtility
    {
        public static double Prediction { get; set; }

        public static double PredictNextDataPoint((string, List<StockPoint>, int) trainingData, (Label, Label) labels)
        {
            // the column name in the temp data set we want to model.
            string targetName = "lastcloseperc";
            // parse the csv file
            ParseCsvDataFile(trainingData, out CsvParser parser, targetName, out double[] targets, out F64Matrix observations);

            // train the model
            RegressionForestModel model = TrainTheModel(targets, observations);
            
            // the variable importance requires the featureNameToIndex
            // from the data set. This mapping describes the relation
            // from column name to index in the feature matrix.
            var featureNameToIndex = parser.EnumerateRows(c => c != targetName)
                .First().ColumnNameToIndex; // todo: may want to change this to the close column

            // Get the variable importance from the model.
            var importances = model.GetVariableImportance(featureNameToIndex);

            // save the model to file for future use
            SaveTheModel(model);

            // default format is xml.
            //var loadedModel = RegressionForestModel.Load(() => new StreamReader(@"randomforest.xml"));

            if (labels.Item1.InvokeRequired || labels.Item2.InvokeRequired)
            {
                labels.Item1.Invoke(new MethodInvoker(delegate () { GetTodayAndTomorrowsPrediction(trainingData, labels, model); }));
            }
            else
            {
                GetTodayAndTomorrowsPrediction(trainingData, labels, model);
            }

            return Prediction;
        }

        private static void SaveTheModel(RegressionForestModel model)
        {
            // default format is xml.
            model.Save(() => new StreamWriter(@"randomforest.xml"));
        }

        private static RegressionForestModel TrainTheModel(double[] targets, F64Matrix observations)
        {
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
            return model;
        }

        private static void ParseCsvDataFile((string, List<StockPoint>, int) trainingData, out CsvParser parser, string targetName, out double[] targets, out SharpLearning.Containers.Matrices.F64Matrix observations)
        {
            // Setup the CsvParser
            parser = new CsvParser(
                () => new StreamReader(trainingData.Item1), separator: ',');

            // read the "close" column, this is the targets for our learner. 
            targets = parser.EnumerateRows(targetName)
                .ToF64Vector();

            // read the feature matrix, all columns except "quality",
            // this is the observations for our learner.
            observations = parser.EnumerateRows(c => c != targetName)
                .ToF64Matrix();
        }

        private static void GetTodayAndTomorrowsPrediction((string, List<StockPoint>, int) trainingData, 
            (Label, Label) labels, RegressionForestModel model)
        {
            // information about the accuracy of the prediction for what happened today
            var traingPointsArray = trainingData.Item2.Take(trainingData.Item2.Count() - 1).ToArray();
            var percent = model.Predict(
                GetLastPattern(traingPointsArray, trainingData.Item3));
            var prediction = percent * double.Parse(traingPointsArray[traingPointsArray.Length - 1].Close);
            var actualPoint = trainingData.Item2.Last();
            labels.Item1.Text = $"Today's Prediction: {prediction} Percent: {Math.Round(percent, 5)}" +
                $" Expected Change: {Math.Round(prediction - double.Parse(traingPointsArray[traingPointsArray.Length - 1].Close), 3)}" +
                $" Off by: {Math.Round(double.Parse(actualPoint.Close) - prediction, 3)}" +
                $" Actual: {actualPoint.Close}";

            // tommorows prediction
            var traingPointsArray2 = trainingData.Item2.ToArray();
            var percent2 = model.Predict(
                GetLastPattern(traingPointsArray2, trainingData.Item3));
            var prediction2 = percent2 * double.Parse(traingPointsArray2[traingPointsArray2.Length - 1].Close);

            labels.Item2.Text = $"Tomorrow's Prediction: {prediction2} Percent: {Math.Round(percent2, 5)}" +
                $" Expected Change: {Math.Round(prediction2 - double.Parse(traingPointsArray2[traingPointsArray2.Length - 1].Close), 3)}";

            Prediction = percent2;
        }

        public static double[] GetLastPattern(StockPoint[] points, int size)
        {
            var observation = new double[size * 13];
            for (int i = 0; i < size; i++)
            {
                observation[i + (i + 0)] = double.Parse(points[points.Length + i - size].Volume);
                // open/high high/low close/low
                observation[i + (i + 1)] = double.Parse(points[points.Length + i - size].Open) / double.Parse(points[points.Length + i - size].High);
                observation[i + (i + 2)] = double.Parse(points[points.Length + i - size].High) / double.Parse(points[points.Length + i - size].Low);
                observation[i + (i + 3)] = double.Parse(points[points.Length + i - size].Close) / double.Parse(points[points.Length + i - size].Low);
                // open/close open/low high/close
                observation[i + (i + 4)] = double.Parse(points[points.Length + i - size].Open) / double.Parse(points[points.Length + i - size].Close);
                observation[i + (i + 5)] = double.Parse(points[points.Length + i - size].Open) / double.Parse(points[points.Length + i - size].Low);
                observation[i + (i + 6)] = double.Parse(points[points.Length + i - size].High) / double.Parse(points[points.Length + i - size].Close);
                // sma
                observation[i + (i + 7)] = (points[points.Length + i - size].MovingAverageTen - points[points.Length + i - size].MovingAverageThirty)
                    / double.Parse(points[points.Length + i - size].Close);
                observation[i + (i + 8)] = ((points[points.Length + i - size].MovingAverageTen + points[points.Length + i - size].MovingAverageThirty)/2)
                    / double.Parse(points[points.Length + i - size].Close);
                // std dev
                observation[i + (i + 9)] = points[points.Length + i - size].StdDev10 / points[points.Length + i - size].MovingAverageTen;
                observation[i + (i + 10)] = points[points.Length + i - size].StdDev30 / points[points.Length + i - size].MovingAverageThirty;
                // bb
                observation[i + (i + 11)] = (points[points.Length + i - size].BBUpper10 - points[points.Length + i - size].BBLower10)
                    / points[points.Length + i - size].MovingAverageTen;
                observation[i + (i + 12)] = (points[points.Length + i - size].BBUpper30 - points[points.Length + i - size].BBLower30)
                    / points[points.Length + i - size].MovingAverageThirty;
            }
            return observation;
        }
    }
}
