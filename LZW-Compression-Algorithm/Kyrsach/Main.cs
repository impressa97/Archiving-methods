using System;
using System.Collections.Generic;
using System.IO;
using LzwCompress;

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
          

            //DODDELAT'
            //foreach(var o in System.IO.Directory.EnumerateFileSystemEntries){
            //if (System.directory.exist(o))
            //{
            // repeat
            //}
            // if (system.io.file.exist(o))
            //{
            //	arch
            //}
            //}
            using (StreamReader sr = new StreamReader(Cpath, System.Text.Encoding.Default))
            {
                str = sr.ReadToEnd();
            }

            List<int> compressed = Lzw.Compress(str);


            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                foreach (var element in compressed)
                {
                    sw.Write(element);
                }

            }

            Dictionary<string, int> tmp = Lzw.DictionaryIni(str);
            //Dictionary<int,string> dic = LzwCompress.DictionaryIni(tmp);

            dstr = Lzw.Decompress(compressed, tmp);

            using (StreamWriter sw = new StreamWriter(Dpath, false, System.Text.Encoding.Default))
            {
                sw.Write(dstr);
            }

            Console.WriteLine("Сжатие успешно");

            Console.ReadKey();

        }




    }
}