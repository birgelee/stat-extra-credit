using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatExtraCredit
{
    class Statistics
    {
        public delegate double PDF(double x);

        private static Random r = new Random();

        public static double RandomFromPDF(PDF distributionFunction, double testingStart, double testingEnd)
        {
            return RandomFromPDFDx(distributionFunction, testingStart, (testingEnd - testingStart) / 10000);
        }


        public static double MeanFromPDF(PDF distributionFunction, double testingStart, double dx)
        {
            double targetRandom = .5;
            double sum = 0;
            int count = 0;
            while (true)
            {
                sum += distributionFunction(testingStart + dx * count) * dx;
                if (sum >= targetRandom)
                {
                    return testingStart + dx * count;
                }

                count++;
                if (count > 50000)
                {
                    Console.WriteLine("Did 50000 iterations and still did not find x.  Current x pos: " + count * dx + ", Target Area: " + targetRandom);
                    return 0;
                }
            }
        }
        public static double RandomFromPDFDx(PDF distributionFunction, double testingStart, double dx)
        {
            double targetRandom = r.NextDouble();
            return InverseFromPDF(distributionFunction, targetRandom, testingStart, dx);
        }

        public static double InverseFromPDF(PDF distributionFunction, double probability, double testingStart, double dx)
        {
            double sum = 0;
            int count = 0;
            while (true)
            {
                sum += distributionFunction(testingStart + dx * count) * dx;
                if (sum >= probability)
                {
                    return testingStart + dx * count;
                }

                count++;
                if (count > 5000000)
                {
                    Console.WriteLine("Did 5000000 iterations and still did not find x.  Current x pos: " + count * dx + ", Target Area: " + probability);
                    return 0;
                }
            }
        }
        public static double GetNormal()
        {
            return GetNormal(0, 1);
        }

        public static double GetNormal(double mean, double standardDeviation)
        {
            return RandomFromPDF(GenerateNormalPDF(mean, standardDeviation), -5 * standardDeviation, 5 * standardDeviation);
        }

        public static double qGetNormal(double mean, double standardDeviation)
        {
            return mean + standardDeviation * qGetNormal();
        }

        // Get normal (Gaussian) random sample with mean 0 and standard deviation 1 (faster than get normal)
        public static double qGetNormal()
        {
            // Use Box-Muller algorithm
            double u1 = Statistics.r.NextDouble();
            double u2 = Statistics.r.NextDouble();
            double r = Math.Sqrt(-2.0 * Math.Log(u1));
            double theta = 2.0 * Math.PI * u2;
            return r * Math.Sin(theta);
        }


        public static PDF GenerateNormalPDF(double mean, double standardDeviation)
        {
            return (x) => (1 / (standardDeviation * Math.Sqrt(2 * Math.PI))) * Math.Pow(Math.E, -Math.Pow(x - mean, 2) / (2 * Math.Pow(standardDeviation, 2)));
        }

        public static PDF GenerateMaxwellDistribution(double a)
        {
            return (x) => Math.Sqrt(2 / Math.PI) * Math.Pow(x, 2) * Math.Pow(Math.E, -Math.Pow(x, 2) / (2 * Math.Pow(a, 2))) / Math.Pow(a, 3);
        }


    }
}