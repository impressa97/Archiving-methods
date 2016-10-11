using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LZW
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	public static class LzwCompress
	{
		/// <summary>
		/// Разархивирование файла
		/// </summary>
		/// <param name="compressed"></param>
		/// <returns></returns>
		 public static string Decompress(List<int> compressed)
        {
            // build the dictionary
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            for (int i = 0; i < 256; i++)
                dictionary.Add(i, ((char)i).ToString());

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

                // new sequence; add it to the dictionary
                dictionary.Add(dictionary.Count, w + entry[0]);

                w = entry;
            }

            return decompressed.ToString();
        }

		/// <summary>
		/// Архивирование файла
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
        public static List<int> Compress(string uncompressed)
        {
            // build the dictionary
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            for (int i = 0; i < 256; i++)
                dictionary.Add(((char)i).ToString(), i);

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
                    
                    Console.Write(wc);
                    compressed.Add(dictionary[w]);
                    dictionary.Add(wc, dictionary.Count);
                    w = c.ToString();
                }
            }

            // write remaining output if necessary
            if (!string.IsNullOrEmpty(w))
                compressed.Add(dictionary[w]);

            return compressed;
        }

	}
}