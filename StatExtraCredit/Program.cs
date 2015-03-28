using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatExtraCredit
{
    class Program
    {
        static void Main(string[] args)
        {
            var sampleCount = 1000000;
            var binSize = 1;

            //Populate Samples.
            var samples = new List<double>();
            for (int i = 0; i < sampleCount; i++)
            {
                samples.Add((Math.Pow(Math.E, Statistics.qGetNormal(3, 1)) + Math.Pow(Math.E, Statistics.qGetNormal(3, 1)) + Math.Pow(Math.E, Statistics.qGetNormal(3, 1))) / 3);
            }
            //Console.WriteLine("binning samples.");
            //Calculate histigram bins.
            var binOcumations = Enumerable.Repeat(0.0, 500).ToList();
            foreach (double d in samples)
            {
                var targetBin = (int)(d / binSize);
                binOcumations[targetBin < binOcumations.Count ? targetBin : binOcumations.Count - 1]++;
            }
            //Console.WriteLine("Sorting samples.");
            //Find 90th percentile.
            samples.Sort();
            var ninedyith = samples[(int)(sampleCount * .9)];
            //Console.WriteLine("Done sorting samples.");

            //Find 90th percentile assuming normal.
            var mean = samples.Average();
            var sumOfSquaresOfDifferences = samples.Select(val => (val - mean) * (val - mean)).Sum();
            var sd = Math.Sqrt(sumOfSquaresOfDifferences / sampleCount);
            var ninedyithFromNormal = Statistics.InverseFromPDF(Statistics.GenerateNormalPDF(mean, sd), .9, -20, 1.0 / 50000.0);

            //Console.WriteLine("Actual 90th percentile: " + ninedyith);
            //Console.WriteLine("Falsely predicted 90th percentile: " + ninedyithFromNormal);
            //Console.WriteLine("Histigram bins: ");
            for (int i = 0; i < binOcumations.Count; i++)
            {
                Console.WriteLine(binOcumations[i]);
            }
            Console.ReadLine();
        }
    }


}
