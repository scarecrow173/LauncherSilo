using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    public interface IStringDistance
    {
        float CalculateNormalizedDistance(string target, string other);
    }
    public class LevenshteinDistance : IStringDistance
    {
        public float CalculateNormalizedDistance(string target, string other)
        {
            char[] sa;
            int n;
            int[] p; //'previous' cost array, horizontally
            int[] d; // cost array, horizontally
            int[] _d; //placeholder to assist in swapping p and d
            sa = target.ToCharArray();
            n = sa.Length;
            p = new int[n + 1];
            d = new int[n + 1];

            int m = other.Length;
            if (n == 0 || m == 0)
            {
                if (n == m)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

            int i; int j;
            char t_j;

            int cost;

            for (i = 0; i <= n; i++)
            {
                p[i] = i;
            }

            for (j = 1; j <= m; j++)
            {
                t_j = other[j - 1];
                d[0] = j;

                for (i = 1; i <= n; i++)
                {
                    cost = sa[i - 1] == t_j ? 0 : 1;
                    d[i] = Math.Min(Math.Min(d[i - 1] + 1, p[i] + 1), p[i - 1] + cost);
                }
                _d = p;
                p = d;
                d = _d;
            }
            return 1.0f - ((float)p[n] / Math.Max(other.Length, sa.Length));
        }
    }

    public class JaroWinklerDistance : IStringDistance
    {
        public float Threshold { get; set; } = 0.7f;
        public float CalculateNormalizedDistance(string target, string other)
        {
            int[] mtp = Matches(target, other);
            float m = mtp[0];
            if (m == 0)
            {
                return 0f;
            }
            float j = ((m / target.Length + m / other.Length + (m - mtp[1]) / m)) / 3;
            float jw = j < Threshold ? j : j + Math.Min(0.1f, 1f / mtp[3]) * mtp[2] * (1 - j);
            return jw;
        }
        private int[] Matches(string target, string other)
        {
            string max, min;
            if (target.Length > other.Length)
            {
                max = target;
                min = other;
            }
            else
            {
                max = other;
                min = target;
            }
            int range = Math.Max(max.Length / 2 - 1, 0);
            int[] matchIndexes = new int[min.Length];
            Fill(matchIndexes, -1);
            bool[] matchFlags = new bool[max.Length];
            int matches = 0;
            for (int mi = 0; mi < min.Length; mi++)
            {
                char c1 = min[mi];
                for (int xi = Math.Max(mi - range, 0), xn = Math.Min(mi + range + 1, max.Length); xi < xn; xi++)
                {
                    if (!matchFlags[xi] && c1 == max[xi])
                    {
                        matchIndexes[mi] = xi;
                        matchFlags[xi] = true;
                        matches++;
                        break;
                    }
                }
            }
            char[] ms1 = new char[matches];
            char[] ms2 = new char[matches];
            for (int i = 0, si = 0; i < min.Length; i++)
            {
                if (matchIndexes[i] != -1)
                {
                    ms1[si] = min[i];
                    si++;
                }
            }
            for (int i = 0, si = 0; i < max.Length; i++)
            {
                if (matchFlags[i])
                {
                    ms2[si] = max[i];
                    si++;
                }
            }
            int transpositions = 0;
            for (int mi = 0; mi < ms1.Length; mi++)
            {
                if (ms1[mi] != ms2[mi])
                {
                    transpositions++;
                }
            }
            int prefix = 0;
            for (int mi = 0; mi < min.Length; mi++)
            {
                if (target[mi] == other[mi])
                {
                    prefix++;
                }
                else
                {
                    break;
                }
            }
            return new int[] { matches, transpositions / 2, prefix, max.Length };
        }
        public static void Fill<T>(T[] a, T val)
        {
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = val;
            }
        }
        public static void Fill<T>(T[] a, int fromIndex, int toIndex, T val)
        {
            if (fromIndex > toIndex)
                throw new ArgumentException("fromIndex(" + fromIndex + ") > toIndex(" + toIndex + ")");
            if (fromIndex < 0)
                throw new ArgumentOutOfRangeException("fromIndex");
            if (toIndex > a.Length)
                throw new ArgumentOutOfRangeException("toIndex");

            for (int i = fromIndex; i < toIndex; i++)
            {
                a[i] = val;
            }
        }
    }
    public class NGramDistance : IStringDistance
    {
        //public int N { get; set; } = 2;
        private const int _n = 2;
        public float CalculateNormalizedDistance(string target, string other)
        {
            int sl = target.Length;
            int tl = other.Length;

            if (sl == 0 || tl == 0)
            {
                if (sl == tl)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

            int cost = 0;
            if (sl < _n || tl < _n)
            {
                for (int i2 = 0, ni = Math.Min(sl, tl); i2 < ni; i2++)
                {
                    if (target[i2] == other[i2])
                    {
                        cost++;
                    }
                }
                return (float)cost / Math.Max(sl, tl);
            }

            char[] sa = new char[sl + _n - 1];
            float[] p; //'previous' cost array, horizontally
            float[] d; // cost array, horizontally
            float[] _d; //placeholder to assist in swapping p and d

            for (int i2 = 0; i2 < sa.Length; i2++)
            {
                if (i2 < _n - 1)
                {
                    sa[i2] = (char)0; //add prefix
                }
                else
                {
                    sa[i2] = target[i2 - _n + 1];
                }
            }
            p = new float[sl + 1];
            d = new float[sl + 1];

            // indexes into strings s and t
            int i; // iterates through target
            int j; // iterates through other

            char[] t_j = new char[_n]; // jth n-gram of t

            for (i = 0; i <= sl; i++)
            {
                p[i] = i;
            }

            for (j = 1; j <= tl; j++)
            {
                //construct t_j n-gram 
                if (j < _n)
                {
                    for (int ti = 0; ti < _n - j; ti++)
                    {
                        t_j[ti] = (char)0; //add prefix
                    }
                    for (int ti = _n - j; ti < _n; ti++)
                    {
                        t_j[ti] = other[ti - (_n - j)];
                    }
                }
                else
                {
                    t_j = other.Substring(j - _n, j - (j - _n)).ToCharArray();
                }
                d[0] = j;
                for (i = 1; i <= sl; i++)
                {
                    cost = 0;
                    int tn = _n;
                    for (int ni = 0; ni < _n; ni++)
                    {
                        if (sa[i - 1 + ni] != t_j[ni])
                        {
                            cost++;
                        }
                        else if (sa[i - 1 + ni] == 0) 
                        {
                            tn--;
                        }
                    }
                    float ec = (float)cost / tn;
                    d[i] = Math.Min(Math.Min(d[i - 1] + 1, p[i] + 1), p[i - 1] + ec);
                }
                _d = p;
                p = d;
                d = _d;
            }

            return 1.0f - (p[sl] / Math.Max(tl, sl));
        }
    }

}
