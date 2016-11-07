using System;

namespace Method
{
    public class Method
    {
        /// <summary>
        /// Constructor for methods
        /// If Zip true then we need to call zip algorithm else unzip.
        /// </summary>
        /// <param name="InputFileStream"> Folder stream with data </param>
        /// <param name="OutputFileStream"> Clear stream for compressed data </param>
        /// <param name="ZipFlag"> Flag of method. If ZipFlag = true then need to call zip method </param>
        public Method(System.IO.Stream InputFileStream, System.IO.Stream OutputFileStream, bool ZipFlag)
        {
            this.BinFileReader = new System.IO.BinaryReader(InputFileStream);
            this.BinFileWriter = new System.IO.BinaryWriter(OutputFileStream);

            if (ZipFlag)
            {
                Compress();
            }
            else
            {
                DeCompress();
            }
        }
        /// <summary>
        /// Method for compressing data from BinFileReader and write into BinFileWriter
        /// </summary>
        private void Compress()
        {
            for (; this.BinFileReader.BaseStream.Position < this.BinFileReader.BaseStream.Length;)
            {
                this.BinFileWriter.Write(this.BinFileReader.ReadByte());
            }
        }
        /// <summary>
        /// Method for decompressing data from BinFileReader and write into BinFileWriter
        /// </summary>
        private void DeCompress()
        {
            for (; this.BinFileReader.BaseStream.Position < this.BinFileReader.BaseStream.Length;)
            {
                this.BinFileWriter.Write(this.BinFileReader.ReadByte());
            }
        }
        /// <summary>
        /// Reader for input stream
        /// </summary>
        private System.IO.BinaryReader BinFileReader
        {
            get;
            set;
        }
        /// <summary>
        /// Writer for input stream
        /// </summary>
        private System.IO.BinaryWriter BinFileWriter
        {
            get;
            set;
        }
        public const UInt16 BufferLengthByte = UInt16.MaxValue;
    }
}
