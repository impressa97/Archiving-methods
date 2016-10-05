using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Resources;
using System.Reflection;
using LzwCompress;

namespace LzwCompress
{
    /// <summary>
    /// Description of MyClass.
    /// </summary>
    public static class Lzw
    {
        /// <summary>
        /// Разархивирование файла
        /// </summary>
        /// <param name="compressed"></param>
        /// <returns></returns>
        public static string Decompress(List<int> compressed, Dictionary<string, int> dic)
        {
           
            Dictionary<int, string> dictionary = DictionaryIni(dic);


            string w = dictionary[compressed[0]];
            compressed.RemoveAt(0);
            StringBuilder decompressed = new StringBuilder(w);

            foreach (int k in compressed)
            {
                string entry = null;
                if (dictionary.ContainsKey(k))
                    entry = dictionary[k];
                else if (k == dictionary.Count)
                    entry = w + w[0];

                decompressed.Append(entry);

                
                dictionary.Add(dictionary.Count, w + entry[0]);

                w = entry;
            }

            return decompressed.ToString();
        }

        /// <summary>
        /// Архивирование файла
        /// </summary>
        /// <param name="uncompressed"></param>
        /// <returns></returns>
        /// 
        public static List<int> Compress(string uncompressed)
        {
            
            Dictionary<string, int> dictionary = DictionaryIni(uncompressed);
          

            string w = string.Empty;

            List<int> compressed = new List<int>();

            foreach (char c in uncompressed)
            {
                string wc = w + c;

                Console.WriteLine(wc);
                if (dictionary.ContainsKey(wc))
                {
                    w = wc;
                }
                else
                {

                   
                    compressed.Add(dictionary[w]);
                    dictionary.Add(wc, dictionary.Count);
                    w = c.ToString();
                }
            }

           
            if (!string.IsNullOrEmpty(w))
                compressed.Add(dictionary[w]);

            return compressed;
        }
        /// <summary>
        /// Выполняет инициальзацию словаря
        /// </summary>
        /// <param name="str"></param>
        /// <returns>Возвращает словарь</returns>
        public static Dictionary<string, int> DictionaryIni(string str)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            string w = string.Empty;

            foreach (var c in str)
            {
                string wc = w + c;
                if (dic.ContainsKey(wc))
                {
                    continue;
                }
                else
                {
                    dic.Add(wc, dic.Count);
                }

            }

            // ResourceManager rm = new ResourceManager("LzwCompress.Properties.Resources", Assembly.GetExecutingAssembly());

            string str1 = LzwCompress.Properties.Resources.dictionary;
            Console.WriteLine(str1);
            return dic;
        }

        /// <summary>
        /// Выполняет инициальзацию словаря
        /// </summary>
        /// <param name="str"></param>
        /// <returns>Возвращает словарь</returns>
        public static Dictionary<int, string> DictionaryIni(Dictionary<string, int> dic)
        {
            Dictionary<int, string> tmp = new Dictionary<int, string>();
          
            List<string> stringlist = new List<string>();

            foreach (var item in dic.Keys)
            {
                stringlist.Add(item);
            }

            int i = 0;
            foreach (var e in dic)
            {
                int tmp1 = dic[stringlist[i]];  
                tmp.Add(tmp1, stringlist[i]);
                i++;
            }


            return tmp;
        }

    }
}