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
        public List<double> Trends = new List<double>();

        private double _alpha;
        private double _beta;


        public DesResult(PointPairList des, List<double> trends, double alpha, double beta, IList<List<int>> normal)
        {
            Trends = trends;
            Des = des;
            _alpha = alpha;
            _beta = beta;

            //Error calculation
            var sum = normal.Select((t, i) => Math.Pow(trends[i] - t[1], 2)).Sum();
            Error = Math.Sqrt(sum / normal.Count - 2);
        }

        public double getAlpha()
        {
            return _alpha;
        }

        public double getBeta()
        {
            return _beta;
        }
    }
}