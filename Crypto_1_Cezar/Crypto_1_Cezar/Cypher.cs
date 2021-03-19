using System;
using System.Collections.Generic;
using System.Text;

namespace Crypto_1_Cezar
{
    public abstract class Cypher
    {
        public static string alfabetEn = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_ ,.!?()";
        public static string alfabetUa = "абвгдеєжзиіїйклмнопрстуфхцчшщьюяФБВГДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ0123456789_ ,.!?()";
        public static int lenOfDev = 65536;
        public abstract string Encrypt(string input, int key, int lang);
        public abstract string Decrypt(string input, int key, int lang);
    }
}
