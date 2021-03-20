using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Crypto_1_Cezar
{
    public class Caesars_code : Cypher
    {
        
        public override string Encrypt(string input, int key, int lang)
        {
            if (input.Length == 0)
                return string.Empty;

            string result = string.Empty;
            string alfabet;
            if (lang == 0)
            {
                foreach (char sym in input)
                {
                    int ch = ((int)sym + key) % lenOfDev;
                    result += (char)ch;
                }
                return result;
            }

            if (lang == 1)
                alfabet = alfabetEn;
            else
                alfabet = alfabetUa;

            foreach (char sym in input)
            {
                if (alfabet.Contains(sym))
                {
                    int ind = (alfabet.IndexOf(sym) + key) % alfabet.Length;
                    if (ind < 0)
                        ind = alfabet.Length + ind;
                    result += alfabet[ind];
                }
                else
                    result += sym;
            }
       
            return result;
        }
        public override string Decrypt(string input, int key, int lang)
        {
            if (input.Length == 0)
                return string.Empty;

            string result = string.Empty;
            if (lang == 0)
            {
                foreach (char sym in input)
                {
                    int ch = ((int)sym - key) % lenOfDev;
                    if (ch < 0)
                        ch = lenOfDev + ch;//+ - 
                    result += (char)ch;
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
                    int ind = (alfabet.IndexOf(sym) - key) % alfabet.Length;
                    if (ind < 0)
                        ind = alfabet.Length + ind;
                    result += alfabet[ind];
                }
                else
                    result += sym;
            }

            return result;
        }
        public override string BroutForseManual(string input, int lang)
        {
            string result = string.Empty;

            if(lang == 0)
            {
                for (int i = 0; i < lenOfDev; i++)
                     result += $"Key {i}\t - {Decrypt(input, i,lang)}\n";
                return result;
            }
            string alfabet;
            if (lang == 1)
                alfabet = alfabetEn;
            else
                alfabet = alfabetUa;

            for (int i = 1; i < alfabet.Length; i++)
                result += $"Key {i}\t - {Decrypt(input, i, lang)}\n";

            return result;
        }
        
        public override string BroutForseAuto(string input, out int key, int lang)
        {
            string[] dict;
            int bestKey = 0;
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
            {
                int currentVerbs = 0;
                string curr = Decrypt(input, i, lang);
                foreach (var item in curr.Split())
                    if (BinarySearch(dict, item.ToUpper(), 0, dict.Length) == 0)
                        currentVerbs++;
                if (currentVerbs > maxVerbs)
                {
                    maxVerbs = currentVerbs;
                    bestKey = i;
                }
            }
            key = bestKey;
            return Decrypt(input, key, lang);
        }
        
        
        public override int HackByFreguency(string input, int lang)
        {
            if (input.Length == 0)
                return 0;// повертаю нульовий ключ
            char toCompare = ' ';
            char mostCommon = MostCommonSymbol(input);

            if (lang == 0)// 65581 element alfabet
                return (int)mostCommon - (int)toCompare;

            string alfabet;
            if (lang == 1)// english
                alfabet = alfabetEn;

            else if (lang == 2)// ukrainian
                alfabet = alfabetUa;
            else
                return 0;

            if (alfabetEn.Contains(mostCommon) && alfabetEn.Contains(toCompare))
            {
                int res = alfabet.IndexOf(mostCommon) - alfabet.IndexOf(toCompare);
                res = res % alfabet.Length;
                if (res < 0)
                    res = alfabet.Length + res;
                return res;
            }
            else
                return 0;
        }

    }
}
