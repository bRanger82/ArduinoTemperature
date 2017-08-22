using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrendCalculation
{
    class Program
    {
        public struct Point
        {
            public double m_y;
            public double m_x;
        }

        public static void CalcValues(List<Point> data, out double slope, out double intercept, out double rSquared)
        {
            double xSum = 0;
            double ySum = 0;
            double xySum = 0;
            double xSqSum = 0;
            double ySqSum = 0;

            foreach (var point in data)
            {
                var x = point.m_x;
                var y = point.m_y;

                xSum += x;
                ySum += y;
                xySum += (x * y);
                xSqSum += (x * x);
                ySqSum += (y * y);
            }

            slope = ((data.Count * xySum) - (xSum * ySum)) /
                         ((data.Count * xSqSum) - (xSum * xSum));

            intercept = ((xSqSum * ySum) - (xSum * xySum)) /
                              ((data.Count * xSqSum) - (xSum * xSum));

            var a = ((data.Count * xySum) - (xSum * ySum));
            var b = (((data.Count * xSqSum) - (xSum * xSum)) *
                         ((data.Count * ySqSum) - (ySum * ySum)));
            rSquared = (a * a) / b;
        }

        static void Main(string[] args)
        {
            var pointList = new List<Point>();
            Random rnd = new Random();

            for(int i = 1; i<=15;i++)
            {
                Point p = new Point();
                p.m_x = i;
                p.m_y = rnd.Next(28, 35);
                Console.WriteLine(" - added point (x) {0}, (y) {1}", p.m_x, p.m_y);
                pointList.Add(p);
            }
            
            double slope, intercept, rsquared;

            CalcValues(pointList, out slope, out intercept, out rsquared);

            Console.WriteLine("slope={0}, intercept={1}, rsquared={2}", slope, intercept, rsquared);

            Console.ReadLine();
        }
    }
}
