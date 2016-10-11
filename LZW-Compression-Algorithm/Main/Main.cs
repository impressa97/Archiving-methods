using System;
using System.Collections.Generic;
using System.IO;


using LZW;

namespace LZW
{
    public class Program
    {
      
        public static void Main(string[] args)
        {
         
        	string path = @"I:\SQLQuery\Compress.lzw";
			string Cpath =  @"I:\SQLQuery\Text.txt";
			string Dpath = @"I:\SQLQuery\Decompress.txt";
        
            string str = string.Empty;
            string dstr = string.Empty;
         
            
            
        	using (StreamReader sr = new StreamReader(Cpath,System.Text.Encoding.Default))
        	{
                str = sr.ReadToEnd();
        	}
			
            List<int> compressed = LzwCompress.Compress(str); 
          
            
            using (StreamWriter sw =  new StreamWriter(path,false,System.Text.Encoding.Default))
        	{	
            	foreach (var element in compressed) 
            	{
            		sw.Write(element);
            	}
            	
        	}
            
            dstr = LzwCompress.Decompress(compressed);
            
            using (StreamWriter sw =  new StreamWriter(Dpath,false,System.Text.Encoding.Default))
        	{	
            	sw.Write(dstr);
        	}

            Console.WriteLine("Сжатие успешно");

            Console.ReadKey();
            
        }


        

    }
}