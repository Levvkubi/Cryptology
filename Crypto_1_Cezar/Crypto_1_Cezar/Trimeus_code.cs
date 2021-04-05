using System;
using System.Collections.Generic;
using System.IO;
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
                    int ind = curr % args[0].Length;
                    res += (int)alfabetEn.IndexOf(args[0][ind]);
                }
            }
            else if (args.Length == 2)
                res = int.Parse(args[0]) * curr + int.Parse(args[1]);
            else if (args.Length == 3)
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
            string[] dict;
            int[] bestKeys = { 0, 0, 0 };
            int maxVerbs = 0;
            using (StreamReader sr = new StreamReader(@"D:\Programming\С#\3Curs_2\Cruptology\Crypto_1_Cezar\Crypto_1_Cezar\Dicshinary.txt"))
            {
                dict = sr.ReadToEnd().Split("\r\n");
            }
            int length;

            if (lang == 0)
                length = lenOfDev;
            else if (lang == 1)
                length = alfabetEn.Length;
            else
                length = alfabetUa.Length;
            for (int i = 1; i < length; i++)
                for (int j = 0; j < length; j++)
                    for (int k = 0; k < length; k++)
                    {
                        int currentVerbs = 0;
                        string curr = Decrypt(input, new string[] { k.ToString(), j.ToString(), i.ToString() }, lang);
                        foreach (var item in curr.Split())
                            if (BinarySearch(dict, item.ToUpper(), 0, dict.Length) == 0)
                                currentVerbs++;
                        if (currentVerbs > maxVerbs)
                        {
                            maxVerbs = currentVerbs;
                            bestKeys = new int[] { k, j, i };
                        }
                    }
            //for (int j = 0; j < length; j++)
            //    for (int k = 0; k < length; k++)
            //    {
            //        int currentVerbs = 0;
            //        string curr = Decrypt(input, new string[] { k.ToString(), j.ToString()}, lang);
            //        foreach (var item in curr.Split())
            //            if (BinarySearch(dict, item.ToUpper(), 0, dict.Length) == 0)
            //                currentVerbs++;
            //        if (currentVerbs > maxVerbs)
            //        {
            //            maxVerbs = currentVerbs;
            //            bestKeys = new int[] { k, j };
            //        }
            //    }
            //for (int k = 0; k < length; k++)
            //{
            //    int currentVerbs = 0;
            //    string curr = Decrypt(input, new string[] { k.ToString() }, lang);
            //    foreach (var item in curr.Split())
            //        if (BinarySearch(dict, item.ToUpper(), 0, dict.Length) == 0)
            //            currentVerbs++;
            //    if (currentVerbs > maxVerbs)
            //    {
            //        maxVerbs = currentVerbs;
            //        bestKeys = new int[] { k };
            //    }
            //}
            keys = new string[bestKeys.Length];
            for (int i = 0; i < bestKeys.Length; i++)
            {
                keys[i] = bestKeys[i].ToString();
            }
            return Decrypt(input, keys, lang);
        }

        public override string BroutForseManual(string input, int lang)
        {
            string result = string.Empty;

            if (lang == 0)
            {
                for (int i = 0; i < lenOfDev; i++)
                    for (int j = 0; j < lenOfDev; j++)
                        for (int k = 0; k < lenOfDev; k++)
                            result += $"Key A {k} B {j} C {i}\t - {Decrypt(input, new string[] { k.ToString(), j.ToString(), i.ToString() }, lang)}\n";
                return result;
            }
            string alfabet;
            if (lang == 1)
                alfabet = alfabetEn;
            else
                alfabet = alfabetUa;

            for (int i = 1; i < alfabet.Length; i++)
                for (int j = 0; j < alfabet.Length; j++)
                    for (int k = 0; k < alfabet.Length; k++)
                        result += $"Key A {k} B {j} C {i}\t - {Decrypt(input, new string[] { k.ToString(), j.ToString(), i.ToString() }, lang)}\n";

            return result;
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
