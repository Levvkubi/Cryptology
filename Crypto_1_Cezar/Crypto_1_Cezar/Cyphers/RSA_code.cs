using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Numerics;

namespace Crypto_1_Cezar.Cyphers
{
    class RSA_code : Cypher
    {
        private long Calculate_e(long d, long m)
        {
            long e = 10;

            while (true)
            {
                if ((e * d) % m == 1)
                    break;
                else
                    e++;
            }

            return e;
        }
        private long Calculate_d(long m)
        {
            long d = m - 1;

            for (long i = 2; i <= m; i++)
                if ((m % i == 0) && (d % i == 0)) 
                {
                    d--;
                    i = 1;
                }

            return d;
        }
        private bool IsTheNumberSimple(long n)
        {
            if (n < 2)
                return false;

            if (n == 2)
                return true;

            for (long i = 2; i < n; i++)
                if (n % i == 0)
                    return false;

            return true;
        }
        public override string BroutForseAuto(string input, out string[] keys, int lang)
        {
            throw new NotImplementedException();
        }

        public override string BroutForseManual(string input, int lang)
        {
            throw new NotImplementedException();
        }

        public override string Decrypt(string input, string[] keys, int lang)
        {
            return "";
        }

        public override string Encrypt(string input, string[] keys, int lang)
        {
            //if (!IsValidKey(keys))
            //    return "";
            long n = long.Parse(keys[0]);
            long e = long.Parse(keys[1]);

            string result = string.Empty;

            BigInteger bi;

            for (int i = 0; i < input.Length; i++)
            {
                int index = alfabetEn.IndexOf(input[i]);

                bi = new BigInteger(index);
                bi = BigInteger.Pow(bi, (int)e);

                BigInteger n_ = new BigInteger((int)n);

                bi = bi % n_;

                result += bi.ToString();
            }

            return result;
        }

        public override string HackByFreguency(string input, int lang)
        {
            throw new NotImplementedException();
        }

        public override void HuckByEnDePair(string encrypted, string decripted, ref string[] args, int lang)
        {
            throw new NotImplementedException();
        }

        public override bool IsValidKey(string[] keys)
        {
            if(keys.Length == 2)
            {
                long a;
                if (long.TryParse(keys[0], out a) && long.TryParse(keys[1], out a))
                    return true;
            } 
            return false;
        }
    }
}
