using System;
using System.Collections.Generic;
using System.Text;

namespace Lutv2
{
    public class ProbVector
    {
        double[] values = new double[52];

        public double this[int i]
        {
            get { return values[i]; }
            set { values[i] = value; }
        }

        public ProbVector()
        {
        }

        public ProbVector(double c)
        {
            for (int i = 0; i < values.Length; i++)
                values[i] = c;
        }

        public ProbVector(ProbVector x, int zeroIndex, double scale)
        {
            for (int i = 0; i < values.Length; i++)
                values[i] = i != zeroIndex ? x.values[i] * scale : 0;
        }

        public double Sum()
        {
            double sum = 0;
            for (int i = 0; i < values.Length; i++)
                sum += values[i];
            return sum;
        }

        public void Add(ProbVector x)
        {
            for (int i = 0; i < values.Length; i++)
                values[i] += x.values[i];
        }

        public void Max(ProbVector x)
        {
            for (int i = 0; i < values.Length; i++)
                values[i] = Math.Max(values[i], x.values[i]);
        }

        public static ProbVector Mul(ProbVector x, ProbVector y)
        {
            ProbVector p = new ProbVector();
            for (int i = 0; i < x.values.Length; i++)
                p.values[i] = x.values[i] * y.values[i];
            return p;
        }

        public void VerifyEqual(ProbVector x)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (Math.Abs(values[i] - x.values[i]) > 0.0000001)
                    throw new Exception(string.Format("fail {0}", Math.Abs(values[i] - x.values[i])));
            }

        }
    }
}
