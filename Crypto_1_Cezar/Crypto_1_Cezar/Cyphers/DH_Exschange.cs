using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Crypto_1_Cezar.Cyphers
{
    class DH_Exschange
    {
        long p;
        long g;
        long s;
        long key;

        public DH_Exschange(long p_,long g_)
        {
            p = p_;
            g = g_;
            Random rand = new Random();
            s = rand.Next((int)p - 1);
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
