using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace StockDisplay
{
    public class TechnicalIndicators
    {
        public Chart Chart { get; set; }

        public TechnicalIndicators(Chart chart)
        {
            Chart = chart;
        }

        public void MovingAverage(List<StockPoint> dataPoints, string series = "MovingAverage", int sizeOfAverage = 10, bool isSpx = false)
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
                    Chart.Series[series].Points.AddXY(dataPoints[i].Date, average);
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

        public void BollingerBand(List<StockPoint> dataPoints, int sizeOfAverage, bool isSpx = false)
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
                    dataPoints[i].BBLower10 = average - stdDev * 2;
                    dataPoints[i].BBUpper10 = average + stdDev * 2;
                }
                else if (sizeOfAverage == 30)
                {
                    dataPoints[i].BBLower30 = average - stdDev * 2;
                    dataPoints[i].BBUpper30 = average + stdDev * 2;
                }

                if (!isSpx)
                {
                    Chart.Series["BBUpper" + sizeOfAverage].Points.AddXY(dataPoints[i].Date, (sizeOfAverage == 10) ? dataPoints[i].BBUpper10 : dataPoints[i].BBUpper30);
                    Chart.Series["BBLower" + sizeOfAverage].Points.AddXY(dataPoints[i].Date, (sizeOfAverage == 10) ? dataPoints[i].BBLower10 : dataPoints[i].BBLower30);
                }
            }
        }

        public void StandardDeviation(List<StockPoint> dataPoints, int sizeOfAverage)
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
    }
}
