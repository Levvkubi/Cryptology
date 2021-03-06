using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Crypto_1_Cezar.Cyphers
{
    class Vigener_code : Cypher
    {
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
            if (!IsValidKey(keys))
                throw new Exception("Invalid data");

            string res = string.Empty;
            string key = keys[0];
            if (lang == 0)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    int curr = (int)input[i] - (int)key[i % key.Length] + lenOfDev;
                    res += (char)(curr % lenOfDev);
                }
                return res;
            }
            string alfabet;
            if (lang == 1)
                alfabet = alfabetEn;
            else
                alfabet = alfabetUa;

            for (int i = 0; i < input.Length; i++)
            {
                if (alfabet.Contains(input[i]) && alfabet.Contains(key[i % key.Length]))
                {
                    int curr = alfabet.IndexOf(input[i]) - alfabet.IndexOf(key[i % key.Length]) + alfabet.Length;
                    res += alfabet[curr % alfabet.Length];
                }
                else
                    res += input[i];
            }

            return res;
        }
        public override string Encrypt(string input, string[] keys, int lang)
        {
            if (!IsValidKey(keys))
                throw new Exception("Invalid data");

            string res = string.Empty;
            string key = keys[0];
            if (lang == 0)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    int curr = (int)input[i] + (int)key[i % key.Length];
                    res += (char)(curr % lenOfDev);
                }
                return res;
            }
            string alfabet;
            if (lang == 1)
                alfabet = alfabetEn;
            else
                alfabet = alfabetUa;

            for (int i = 0; i < input.Length; i++)
            {
                if (alfabet.Contains(input[i]) && alfabet.Contains(key[i % key.Length]))
                {
                    int curr = alfabet.IndexOf(input[i]) + alfabet.IndexOf(key[i % key.Length]);
                    res += alfabet[curr % alfabet.Length];
                }
                else
                    res += input[i];
            }

            return res;
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
            if (keys.Length > 0 && keys[0].Length > 0)
                return true;
            else
                return false;
        }
    }
}
