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
            var sum = normal.Select((item, index) => Math.Pow(item[1] - ses[index].Y, 2)).Sum();
            Error = Math.Sqrt(sum /  (ses.Count - 1));
        }

        public double GetAlpha()
        {
            return _alpha;
        }
    }
}