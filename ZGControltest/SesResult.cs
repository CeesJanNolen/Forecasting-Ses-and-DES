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
        private double alpha;

        public SesResult(PointPairList ses, double alpha, IList<List<int>> normal)
        {
            Ses = ses;
            this.alpha = alpha;

            //Error calculation
            var sum = normal.Select((t, i) => Math.Pow(t[1] - ses[i].Y, 2)).Sum();
            Error = Math.Sqrt(sum - ses.Count - 1);
        }

        public double getAlpha()
        {
            return alpha;
        }
    }
}