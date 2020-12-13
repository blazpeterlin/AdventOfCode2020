using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AOC.Common
{
    class ModuloMaths
    {
        public static BigInteger ChineseRemainderTheorem(long[] n, long[] a)
        {
            BigInteger prod = n.Aggregate(new BigInteger(1), (i, j) => i * j);
            BigInteger sm = 0;
            for (int i = 0; i < n.Length; i++)
            {
                BigInteger p = prod / n[i];
                sm += a[i] * ModularMultiplicativeInverse(p, n[i]) * p;
            }
            return sm % prod;
        }

        public static BigInteger ModularMultiplicativeInverse(BigInteger a, BigInteger mod)
        {
            BigInteger b = a % mod;
            for (BigInteger x = 1; x < mod; x++)
            {
                if ((b * x) % mod == 1)
                {
                    return x;
                }
            }
            return 1;
        }
    }
}

