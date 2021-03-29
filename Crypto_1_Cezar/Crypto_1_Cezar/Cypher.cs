using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crypto_1_Cezar
{
    public abstract class Cypher
    {
        public static string alfabetEn = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_ ,.!?()";
        public static string alfabetUa = "абвгдеєжзиіїйклмнопрстуфхцчшщьюяФБВГДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ0123456789_ ,.!?()";
        public static int lenOfDev = 65536;
        public abstract string Encrypt(string input, string[] keys, int lang);
        //перші два шифри можна було б замінити одним статичним методом з одним лиш вирахуванням ссуву яке відрізняється але надалі це здається не підійде
        public abstract string Decrypt(string input, string[] keys, int lang);
        public abstract string BroutForseManual(string input, int lang);
        public abstract string BroutForseAuto(string input, out string[] keys, int lang);
        protected int BinarySearch(string[] array, string searchedValue, int left, int right)
        {
            while (left <= right)
            {
                var middle = (left + right) / 2;
                if (middle >= array.Length)
                    return -1;

                if (searchedValue.Equals(array[middle]))
                    return 0;
                else if (searchedValue.CompareTo(array[middle]) < 0)
                    right = middle - 1;
                else
                    left = middle + 1;
            }
            return -1;
        }
        public string GetFrequency(string input)
        {
            Dictionary<char, int> frequencies = new Dictionary<char, int>();
            foreach (var item in input)
            {
                if (frequencies.ContainsKey(item))
                    frequencies[item]++;
                else
                    frequencies.Add(item, 1);
            }
            var ordered = frequencies.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            string resK = string.Empty;
            string resV = string.Empty;

            foreach (var item in ordered.Reverse())
            {
                if (item.Key == '\n')
                    resK += $"'\\n'\t";
                else if (item.Key == '\t')
                    resK += $"'\\'t'\t";
                else if (item.Key == '\r')
                    resK += $"'\\'r'\t";
                else
                    resK += $"'{item.Key}'\t";
                resV += $"{Math.Round((float)item.Value / input.Length * 100, 1)}%\t";
            }
            return resK + '\n' + resV;
        }
        protected char MostCommonSymbol(string input)
        {
            if (input.Length == 0)
                throw new ArgumentNullException();
            char rez = input[0];
            int count = 1;
            Dictionary<char, int> frequencies = new Dictionary<char, int>();
            foreach (var item in input)
            {
                if (frequencies.ContainsKey(item))
                {
                    frequencies[item]++;
                    if (frequencies[item] > count)
                    {
                        rez = item;
                        count = frequencies[item];
                    }
                }
                else
                    frequencies.Add(item, 1);
            }
            return rez;
        }
        public abstract int HackByFreguency(string input, int lang);
        public abstract bool IsValidKey(string[] keys);
        
    }
}
