using System;
using System.Collections.Generic;
using System.IO;

namespace LZW
{
    public class Program
    {

        public static void Main()
        {

            string path = @"D:\SQLQuery\Compress.lzw"; //Заархивированный
            string Cpath = @"D:\SQLQuery\test4"; //Исходный
            string Dpath = @"D:\SQLQuery\Decompress";//разархивированный

            string str = string.Empty;
            string dstr = string.Empty;



            using (StreamReader sr = new StreamReader(Cpath, System.Text.Encoding.UTF8))
            {
                str = sr.ReadToEnd();
            }

            List<int> compressed = LzwCompress.Compress(str);


            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.UTF8))
            {
                foreach (var element in compressed)
                {
                    sw.Write(element);
                }

            }

            Dictionary<string, int> tmp = LzwCompress.DictionaryIni(str);


            dstr = LzwCompress.Decompress(compressed, tmp);

            using (StreamWriter sw = new StreamWriter(Dpath, false, System.Text.Encoding.UTF8))
            {
                sw.Write(dstr);
            }

            Console.WriteLine("Сжатие успешно");

            Console.ReadKey();

        }




    }
}