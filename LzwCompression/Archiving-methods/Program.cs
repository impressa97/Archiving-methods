using System;
using System.IO;
using Method;


namespace Archiving_methods
{
    class Program
    {
        static void Main(string[] args)
        {

            string path = @"C:\Test\test.txt";  //Отсюда берем
            string Spath = @"C:\Test\Compress.txt"; //Сюда кладем
            string Dpath = @"C:\Test\DeCompress.txt";// Сюда возвращаем
            //Arc
            FileStream InputFileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            FileStream OutputFileStream = new FileStream(Spath, FileMode.Open, FileAccess.Write);
            Method.Method me = new Method.Method(InputFileStream, OutputFileStream, true);
            InputFileStream.Close();
            OutputFileStream.Close();

            //Dearc
            FileStream InputFileStream1 = new FileStream(Spath, FileMode.Open, FileAccess.Read);
            FileStream OutputFileStream1 = new FileStream(Dpath, FileMode.Open, FileAccess.Write);
            Method.Method me1 = new Method.Method(InputFileStream1, OutputFileStream1, false);
            Console.ReadKey();
        }
    }
}
