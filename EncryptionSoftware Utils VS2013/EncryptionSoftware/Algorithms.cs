using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EncryptionSoftware
{
    class Algorithms
    {

        public static string Encrypt(string text, string key, string key2)
        {
            //text = text.ToLower();
            if (!key.Contains(" ") && !key2.Contains(" "))
                if (key.Length >= 5)
                {
                    text = text.Replace('a', key[0]);
                    text = text.Replace('e', key[1]);
                    text = text.Replace('i', key[2]);
                    text = text.Replace('o', key[3]);
                    text = text.Replace('u', key[4]);
                    switch (key2.Length)
                    {
                        case 1:
                            text = text.Replace('b', key2[0]);
                            break;
                        case 2:
                            text = text.Replace('b', key2[0]);
                            text = text.Replace('c', key2[1]);
                            break;
                        case 3:
                            text = text.Replace('b', key2[0]);
                            text = text.Replace('c', key2[1]);
                            text = text.Replace('r', key2[2]);
                            break;
                        case 4:
                            text = text.Replace('b', key2[0]);
                            text = text.Replace('c', key2[1]);
                            text = text.Replace('r', key2[2]);
                            text = text.Replace('p', key2[3]);
                            break;
                        case 5:
                            text = text.Replace('b', key2[0]);
                            text = text.Replace('c', key2[1]);
                            text = text.Replace('r', key2[2]);
                            text = text.Replace('p', key2[3]);
                            text = text.Replace('m', key2[4]);
                            break;
                        default: break;
                    }
                    return text;
                }
            return "Key is invalid!";
        }

        public static string Decrypt(string text, string key, string key2)
        {
            if (!key.Contains(" ") && !key2.Contains(" "))
                if (key.Length >= 5)
                {
                    text = text.Replace(key[0], 'a');
                    text = text.Replace(key[1], 'e');
                    text = text.Replace(key[2], 'i');
                    text = text.Replace(key[3], 'o');
                    text = text.Replace(key[4], 'u');
                    switch (key2.Length)
                    {
                        case 1:
                            text = text.Replace(key2[0], 'b');
                            break;
                        case 2:
                            text = text.Replace(key2[0], 'b');
                            text = text.Replace(key2[1], 'c');
                            break;
                        case 3:
                            text = text.Replace(key2[0], 'b');
                            text = text.Replace(key2[1], 'c');
                            text = text.Replace(key2[2], 'r');
                            break;
                        case 4:
                            text = text.Replace(key2[0], 'b');
                            text = text.Replace(key2[1], 'c');
                            text = text.Replace(key2[2], 'r');
                            text = text.Replace(key2[3], 'p');
                            break;
                        case 5:
                            text = text.Replace(key2[0], 'b');
                            text = text.Replace(key2[1], 'c');
                            text = text.Replace(key2[2], 'r');
                            text = text.Replace(key2[3], 'p');
                            text = text.Replace(key2[4], 'm');
                            break;
                        default: break;
                    }
                    return text;
                }

            return "Key is invalid!";
        }
    }
}
