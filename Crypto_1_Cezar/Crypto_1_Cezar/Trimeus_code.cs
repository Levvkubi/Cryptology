using System;
using System.Collections.Generic;
using System.Text;

namespace Crypto_1_Cezar
{
    class Trimeus_code : Cypher
    {
        private int getK(int curr, string[] args)
        {
            //if (!IsValidKey(args))
            //    return 0;
            int res = 0;
            if(args.Length == 1)
            {
                if(int.TryParse(args[0],out int a))
                    res = int.Parse(args[0]);
                else
                {
                    // realisation gaslo code
                }
            }
            else if (args.Length == 2)
                res = int.Parse(args[0]) * curr + int.Parse(args[1]);
            else if (args.Length == 2)
                res = int.Parse(args[0]) * curr * curr + int.Parse(args[1]) * curr + int.Parse(args[2]);
            return res;
        }
        public override string Decrypt(string input, string[] keys, int lang)
        {
            if (input.Length == 0)
                return string.Empty;

            int i = 1;
            string result = string.Empty;

            if (lang == 0)
            {
                foreach (char sym in input)
                {
                    int ch = ((int)sym - getK(i, keys)) % lenOfDev;
                    if (ch < 0)
                        ch = lenOfDev + ch;//+ - 
                    result += (char)ch;
                    i++;
                }

                return result;
            }

            string alfabet;
            if (lang == 1)
                alfabet = alfabetEn;
            else
                alfabet = alfabetUa;

            foreach (char sym in input)
            {
                if (alfabet.Contains(sym))
                {
                    int ind = (alfabet.IndexOf(sym) - getK(i,keys)) % alfabet.Length;
                    if (ind < 0)
                        ind = alfabet.Length + ind;
                    result += alfabet[ind];
                }
                else
                    result += sym;
                i++;
            }

            return result;
        }

        public override string Encrypt(string input, string[] keys, int lang)
        {
            if (input.Length == 0)
                return string.Empty;
            int i = 1;
            string result = string.Empty;
            if (lang == 0)
            {
                foreach (char sym in input)
                {
                    int ch = ((int)sym + getK(i,keys)) % lenOfDev;
                    result += (char)ch;
                    i++;
                }
                return result;
            }

            string alfabet;
            if (lang == 1)
                alfabet = alfabetEn;
            else
                alfabet = alfabetUa;

            foreach (char sym in input)
            {
                if (alfabet.Contains(sym))
                {
                    int ind = (alfabet.IndexOf(sym) + getK(i,keys)) % alfabet.Length;
                    if (ind < 0)
                        ind = alfabet.Length + ind;
                    result += alfabet[ind];
                }
                else
                    result += sym;
                i++;
            }

            return result;
        }
        public override string BroutForseAuto(string input, out string[] keys, int lang)
        {
            throw new NotImplementedException();
        }

        public override string BroutForseManual(string input, int lang)
        {
            throw new NotImplementedException();
        }

        public override int HackByFreguency(string input, int lang)
        {
            throw new NotImplementedException();
        }

        public override bool IsValidKey(string[] keys)
        {
            if (keys.Length == 1)
                return true;
            else if (keys.Length == 2 || keys.Length == 3)
            {
                foreach (var item in keys)
                    if (!int.TryParse(item, out int a))
                        return false;

                return true;
            }
            else
                return false;
        }
    }
}
