using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Numerics;
using Rand = System.Security.Cryptography.RandomNumberGenerator;
using System.IO;
using System.Windows;

namespace Crypto_1_Cezar.Cyphers
{
    class RSA_code : Cypher
    {
        private static bool IsTheNumberSimple(BigInteger n)
        {
            if (n < 2)
                return false;

            if (n == 2)
                return true;

            for (BigInteger i = 2; i < n; i++)
                if (n % i == 0)
                    return false;

            return true;
        }

        private static BigInteger Calculate_d(BigInteger modulus, BigInteger number)
        {
            var initialModulus = modulus;
            var y = BigInteger.Zero;
            var x = BigInteger.One;

            if (modulus.IsOne)
            {
                return BigInteger.Zero;
            }

            while (number > BigInteger.One)
            {
                var q = number / modulus;

                var temp = modulus;
                modulus = number % modulus;
                number = temp;

                temp = y;
                y = x - q * y;
                x = temp;
            }

            if (x < BigInteger.Zero)
            {
                x += initialModulus;
            }

            return x;
        }

        public static BigInteger __gcd(BigInteger a, BigInteger b)
        {
            if (a == 0 || b == 0)
                return 0;

            if (a == b)
                return a;

            if (a > b)
                return __gcd(a - b, b);

            return __gcd(a, b - a);
        }

        public static bool IsCoprime(BigInteger a, BigInteger b)
        {

            if (__gcd(a, b) == 1)
                return true;
            else
                return false;
        }

        private static BigInteger Calculate_e(BigInteger fn)
        {
            BigInteger e = 7;

            while (!IsCoprime(fn, e))
            {
                e++;
            }

            return e;
        }
        //private static long Calculate_e(long d, long m)
        //{
        //    long e = 10;

        //    while (true)
        //    {
        //        if ((e * d) % m == 1)
        //            break;
        //        else
        //            e++;
        //    }

        //    return e;
        //}
        //private static long Calculate_d(long m)
        //{
        //    long d = m - 1;

        //    for (long i = 2; i <= m; i++)
        //        if ((m % i == 0) && (d % i == 0)) 
        //        {
        //            d--;
        //            i = 1;
        //        }

        //    return d;
        //}
        //private static bool IsTheNumberSimple(long n)
        //{
        //    if (n < 2)
        //        return false;

        //    if (n == 2)
        //        return true;

        //    for (long i = 2; i < n; i++)
        //        if (n % i == 0)
        //            return false;

        //    return true;
        //}
        public static void GenerateKeys(out BigInteger n, out BigInteger e_, out BigInteger d)
        {
            Random random = new Random();
            BigInteger p;
            BigInteger q;
            n = 0;
            e_ = 0;
            d = 0;

            string[] dict;
            using (StreamReader sr = new StreamReader(@"D:\Programming\С#\3Curs_2\Cruptology\Crypto_1_Cezar\Crypto_1_Cezar\SimpleNumbers.txt"))
            {
                dict = sr.ReadToEnd().Split("\r\n");
            }
            //p = new BigInteger(long.Parse(dict[random.Next(dict.Length)]));
            //q = new BigInteger(long.Parse(dict[random.Next(dict.Length)]));
            p = 17;
            q = 11;
            if (IsTheNumberSimple(p) && IsTheNumberSimple(q))
            {
                n = p * q;
                BigInteger fn = (p - 1) * (q - 1);
                e_ = Calculate_e(fn);
                d = Calculate_d(fn, e_);
            }
            else
                MessageBox.Show("p або q - не прості числа!", "Warning!");
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
            long temp;
            foreach (var item in input.Split())
                if(!long.TryParse(item,out temp))
                    if(item !="") 
                        return "";


            string result = "";

            long n = long.Parse(keys[0]);
            long d = long.Parse(keys[1]);

            BigInteger bi;

            foreach (string item in input.Split())
            {
                if (item == "")
                    continue;
                bi = new BigInteger(Convert.ToInt64(item));
                int index = int.Parse(modular_pow(bi, d, n).ToString());

                result += alfabetEn[index];
            }

            return result;
        }
        BigInteger modular_pow(BigInteger bi, long index_n, BigInteger modulus)
        {
            BigInteger res = 1;
            for (int i = 0; i < index_n; i++)
            {
                res = (res * bi) % modulus;
            }
            return res;
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

                result += bi.ToString() + ' ';
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
