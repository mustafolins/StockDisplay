using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockDisplay
{
    public static class CsvUtilities
    {
        public static (string fileName, List<StockPoint> stockPoints, int sizeOfPattern) CreateTrainingDataFile(
            List<StockPoint> dataJPoints, ProgressBar currentProgress, int sizeOfPattern, int curPattern)
        {
            var fileName = "traingdatatemp" + curPattern + ".csv";
            using (var sw = new StreamWriter(fileName))
            {
                sw.WriteLine(GetCsvColumnHeader(sizeOfPattern));
                var trainablePoints = dataJPoints.ToArray();
                for (int i = 0; i < trainablePoints.Length - sizeOfPattern; i++)
                {
                    var point = trainablePoints[i];
                    sw.WriteLine(GetCsvDataRow(trainablePoints, i, sizeOfPattern));

                    // update progress bar
                    if (currentProgress.InvokeRequired)
                    {
                        currentProgress.Invoke(
                            new MethodInvoker(
                                delegate () {
                                    currentProgress.Value = (int)((double)(i + 1) / (double)(trainablePoints.Length - sizeOfPattern) * 100);
                                }));
                    }
                    else
                    {
                        currentProgress.Value = i + 1 / trainablePoints.Length - sizeOfPattern;
                    }
                }
            }
            return (fileName, dataJPoints, sizeOfPattern);
        }

        public static string GetCsvColumnHeader(int size)
        {
            var result = "";
            for (int i = 0; i < size; i++)
            {
                result += $"volume{i}," +
                    $"openhigh{i},highlow{i},closelow{i},openclose{i},openlow{i},highclose{i}," +
                    $"ma10abovema30{i},maa{i},stddev10{i},stddev30{i}," +
                    $"bb10{i},bb30{i},";
            }
            return result + "lastcloseperc";
        }

        public static string GetCsvDataRow(StockPoint[] trainablePoints, int i, int size = 3)
        {
            var result = "";
            for (int j = 0; j < size; j++)
            {
                // open high low close volume
                result += $"{trainablePoints[i + j].Volume}," +
                    // open/high high/low close/low open/close open/low high/close
                    $"{double.Parse(trainablePoints[i + j].Open) / double.Parse(trainablePoints[i + j].High)}," +
                    $"{double.Parse(trainablePoints[i + j].High) / double.Parse(trainablePoints[i + j].Low)}," +
                    $"{double.Parse(trainablePoints[i + j].Close) / double.Parse(trainablePoints[i + j].Low)}," +
                    $"{double.Parse(trainablePoints[i + j].Open) / double.Parse(trainablePoints[i + j].Close)}," +
                    $"{double.Parse(trainablePoints[i + j].Open) / double.Parse(trainablePoints[i + j].Low)}," +
                    $"{double.Parse(trainablePoints[i + j].High) / double.Parse(trainablePoints[i + j].Close)}," +
                    // sma
                    $"{(trainablePoints[i + j].MovingAverageTen - trainablePoints[i + j].MovingAverageThirty) / double.Parse(trainablePoints[i + j].Close)}," +
                    $"{((trainablePoints[i + j].MovingAverageTen + trainablePoints[i + j].MovingAverageThirty) / 2.0) / double.Parse(trainablePoints[i + j].Close)}," +
                    // std deviation
                    $"{trainablePoints[i + j].StdDev10 / trainablePoints[i + j].MovingAverageTen}," +
                    $"{trainablePoints[i + j].StdDev30 / trainablePoints[i + j].MovingAverageThirty}," +
                    // bollinger band
                    // todo: maybe the distance between the bb upper and lower isn't the best metric
                    $"{(trainablePoints[i + j].BBUpper10 - trainablePoints[i + j].BBLower10)}," +
                    $"{(trainablePoints[i + j].BBUpper30 - trainablePoints[i + j].BBLower30)},";
            }
            // get the percent growth and add it to data to be trained lastcloseperc column
            result += $"{double.Parse(trainablePoints[i + size].Close) / double.Parse(trainablePoints[i + size - 1].Close)}";
            return result;
        }
    }
}
