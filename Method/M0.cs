using System;
using System.Linq;
using System.IO.Compression;

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
            this.BinFileReader.BaseStream.Position = 0;
            this.BinFileWriter.BaseStream.Position = 0;

            for (; this.BinFileReader.BaseStream.Position < this.BinFileReader.BaseStream.Length; )
            {
                this.StreamPositionBuffer = this.BinFileReader.BaseStream.Position;

                for (; this.BinFileReader.BaseStream.Position < this.BinFileReader.BaseStream.Length; )
                {
                    this.ReadedByte = this.BinFileReader.ReadByte();

                    if (this.IndexOfEquals == null)
                    {
                        this.IndexOfEquals = new System.Collections.Generic.List<Int32>();

                        this.IndexOfEquals = this.SlidingWindow.Select((x, i) => (x == this.ReadedByte) ? i : -1).Where(x => x != -1).ToList();
                    }
                    else
                    {
                        foreach (Int32 x in IndexOfEquals)
                        {
                            if ((x + this.Offset) < SlidingWindow.Capacity)
                            {
                                if (this.SlidingWindow[x + this.Offset] != this.ReadedByte)
                                {
                                    this.IndexForRemove.Add(x);
                                }
                            }
                            else
                            {
                                /// Тут не правельно
                                this.IndexForRemove.Add(x);
                            }
                        }

                        if ((this.IndexOfEquals.Count - this.IndexForRemove.Count) > 0)
                        {
                            foreach (Int32 x in IndexForRemove)
                            {
                                this.IndexOfEquals.RemoveAt(x);
                            }
                        }
                    }

                    ++this.Offset;
                }
            }
        }
        /// <summary>
        /// Method for decompressing data from BinFileReader and write into BinFileWriter
        /// </summary>
        private void DeCompress()
        {
            this.BinFileReader.BaseStream.Position = 0;
            this.BinFileWriter.BaseStream.Position = 0;
            for (; this.BinFileReader.BaseStream.Position < this.BinFileReader.BaseStream.Length;)
            {
                if ((this.BinFileReader.BaseStream.Position + 8) < this.BinFileReader.BaseStream.Length)
                {
                    this.BinFileWriter.Write(this.BinFileReader.ReadUInt64());
                }
                else
                {
                    this.BinFileWriter.Write(this.BinFileReader.ReadByte());
                }
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
        private byte ReadedByte
        {
            get;
            set;
        }
        private Int32 Offset
        {
            get;
            set;
        }
        private Int64 StreamPositionBuffer
        {
            get;
            set;
        }
        private System.Collections.Generic.List<byte> SlidingWindow
        {
            get;
            set;
        }
        private System.Collections.Generic.List<Int32> IndexOfEquals
        {
            get;
            set;
        }
        private System.Collections.Generic.List<Int32> IndexForRemove
        {
            get;
            set;
        }
        public const UInt16 BufferLengthByte = 1024;
    }
}
