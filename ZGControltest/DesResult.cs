using System;
using System.Collections.Generic;
using System.Linq;
using ZedGraph;

namespace forecasting
{
    public class DesResult
    {
        public double Error;
        public PointPairList Des;
        public List<double> Trends;

        private readonly double _alpha;
        private readonly double _beta;


        public DesResult(PointPairList des, List<double> trends, double alpha, double beta, ICollection<List<int>> normal)
        {
            Trends = trends;
            Des = des;
            _alpha = alpha;
            _beta = beta;

            //Error calculation
            var sum = normal.Select((item, index) => Math.Pow(trends[index] - item[1], 2)).Sum();
            Error = Math.Sqrt(sum / (normal.Count - 2));
        }

        public double GetAlpha()
        {
            return _alpha;
        }

        public double GetBeta()
        {
            return _beta;
        }
    }
}