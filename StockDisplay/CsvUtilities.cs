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
        public static (string, List<StockPoint>, int) CreateTrainingDataFile(
            List<StockPoint> dataJPoints, StockView view, int sizeOfPattern)
        {
            var fileName = "traingdatatemp.csv";
            using (var sw = new StreamWriter(fileName))
            {
                sw.WriteLine(GetCsvColumnHeader(sizeOfPattern));
                var trainablePoints = dataJPoints.ToArray();
                for (int i = 0; i < trainablePoints.Length - sizeOfPattern; i++)
                {
                    var point = trainablePoints[i];
                    sw.WriteLine(GetCsvDataRow(trainablePoints, i, sizeOfPattern));
                    if (view.CurrentProgress.InvokeRequired)
                    {
                        view.Invoke(
                            new MethodInvoker(
                                delegate () {
                                    view.CurrentProgress.Value = (int)((double)(i + 1) / (double)(trainablePoints.Length - sizeOfPattern) * 100);
                                }));
                    }
                    else
                    {
                        view.CurrentProgress.Value = i + 1 / trainablePoints.Length - sizeOfPattern;
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
                result += $"open{i},high{i},low{i},close{i},volume{i},ma10abovema30{i},maa{i},stddev10{i},stddev30{i},";
            }
            return result + "lastcloseperc";
        }

        public static string GetCsvDataRow(StockPoint[] trainablePoints, int i, int size = 3)
        {
            var result = "";
            for (int j = 0; j < size; j++)
            {
                result += $"{trainablePoints[i + j].Open},{trainablePoints[i + j].High},{trainablePoints[i + j].Low}," +
                    $"{trainablePoints[i + j].Close},{trainablePoints[i + j].Volume}," +
                    $"{trainablePoints[i + j].MovingAverageTen - trainablePoints[i + j].MovingAverageThirty}," +
                    $"{(trainablePoints[i + j].MovingAverageTen + trainablePoints[i + j].MovingAverageThirty) / 2.0}," +
                    $"{trainablePoints[i + j].StdDev10},{trainablePoints[i + j].StdDev30},";
            }
            result += $"{double.Parse(trainablePoints[i + size].Close) / double.Parse(trainablePoints[i + size - 1].Close)}";
            return result;
        }
    }
}
