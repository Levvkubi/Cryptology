using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Crypt_6_Kos
{
    class DH_Algorythm
    {
        long p;
        long g;
        long s;

        public DH_Algorythm(long p_, long g_,long s_)
        {
            p = p_;
            g = g_;
            s = s_;
        }
        public long getSecretKey()
        {
            return s;
        }
        public long getMiddleKey()
        {
            long res;
            res = long.Parse(modular_pow(g, s, p).ToString());

            return res;
        }
        public long getFinKey(long oherKey)
        {
            long res;
            res = long.Parse(modular_pow(oherKey, s, p).ToString());
            return res;
        }
        BigInteger modular_pow(BigInteger bas, long index_n, BigInteger modulus)
        {
            BigInteger res = 1;
            for (int i = 0; i < index_n; i++)
            {
                res = (res * bas) % modulus;
            }
            return res;
        }
    }
}
