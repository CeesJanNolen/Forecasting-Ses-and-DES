using System;
using System.Collections.Generic;
using System.Linq;
using ZedGraph;

namespace forecasting
{
    public class SesResult
    {
        public double Error;
        public PointPairList Ses;
        private readonly double _alpha;

        public SesResult(PointPairList ses, double alpha, IEnumerable<List<int>> normal)
        {
            Ses = ses;
            _alpha = alpha;

            //Error calculation
            var sum = normal.Select((t, i) => Math.Pow(t[1] - ses[i].Y, 2)).Sum();
            Error = Math.Sqrt(sum - ses.Count - 1);
        }

        public double GetAlpha()
        {
            return _alpha;
        }
    }
}