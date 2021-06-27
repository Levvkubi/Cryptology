using System.IO;

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

        public override string HackByFreguency(string input, int lang)
        {
            char toCompare = ' ';
            string[] dict;
            string bestKey = string.Empty;
            int bestSp = 0;
            using (StreamReader sr = new StreamReader(@"D:\Programming\С#\3Curs_2\Cruptology\Crypto_1_Cezar\Crypto_1_Cezar\Dicshinary.txt"))
            {
                dict = sr.ReadToEnd().Split("\r\n");
            }
            string rez = string.Empty;
            int len = 10;
            if (len > input.Length)
                len = input.Length;
            for (int i = 1; i < len; i++)
            {
                string currKey = string.Empty;
                for (int j = 0; j < i; j++)
                {
                    string currChars = string.Empty;
                    for (int k = j; k < input.Length; k+=i)
                    {
                        currChars += input[k];
                    }
                    char mostCommon = MostCommonSymbol(currChars);
                    if (alfabetEn.Contains(mostCommon) && alfabetEn.Contains(toCompare))
                    {
                        int res = alfabetEn.IndexOf(mostCommon) - alfabetEn.IndexOf(toCompare);
                        
                        res = res % alfabetEn.Length;
                        if (res < 0)
                            currKey += alfabetEn[alfabetEn.Length + res];
                        else
                            currKey += alfabetEn[res];
                    }
                    else
                        return currKey += alfabetEn[0];
                }
                if (currKey.Length >= 2)
                    currKey = currKey[currKey.Length - 1] + currKey.Substring(0, currKey.Length - 1);
                bestKey += currKey + "\n";
                //if (bestSp < CheckVerbs(dict, Decrypt(input, new string[] { currKey }, lang)))
                //    bestKey = currKey;
            }
            return bestKey;
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

        public override void HuckByEnDePair(string encrypted, string decripted, ref string[] args, int lang)
        {
            if (encrypted.Length != decripted.Length)
                throw new InvalidDataException();

            if(args[0] == "0")
            {
                string ress = string.Empty;
                for (int i = 0; i < encrypted.Length; i++)
                {
                    if (alfabetEn.Contains(encrypted[i]) && alfabetEn.Contains(decripted[i]))
                    {
                        int res = alfabetEn.IndexOf(encrypted[i]) - alfabetEn.IndexOf(decripted[i]);

                        res = res % alfabetEn.Length;
                        if (res < 0)
                            ress += alfabetEn[alfabetEn.Length + res];
                        else
                            ress += alfabetEn[res];
                    }
                    else
                        ress += alfabetEn[0];
                    string curr = ress;
                    if (ress.Length >= 2)
                        curr = ress[ress.Length - 1] + ress.Substring(0, ress.Length - 1);
                    
                    if(Decrypt(encrypted, new string[] { curr }, lang) == decripted)
                    {
                        args = new string[] { curr };
                        return;
                    }
                    
                }
                args = new string[] { ress };
            }
            else
            {
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
                            string curr = Decrypt(encrypted, new string[] { k.ToString(), j.ToString(), i.ToString() }, lang);

                            if (decripted == curr)
                            {
                                int[] bestKeys = new int[] { k, j, i };
                                args = new string[bestKeys.Length];
                                for (int l = 0; l < bestKeys.Length; l++)
                                {
                                    args[l] = bestKeys[l].ToString();
                                }
                                return;
                            }
                        }
                args = new string[] { "0", "0", "0" };

                
            }
               
        }
    }
}
