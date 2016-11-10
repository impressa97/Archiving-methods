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

            this.SlidingWindow = new System.Collections.Generic.List<byte>();

            for (; this.BinFileReader.BaseStream.Position < this.BinFileReader.BaseStream.Length;)
            {
                /// Запомнили стартовую позицию указателя
                this.StreamPositionBuffer = this.BinFileReader.BaseStream.Position;
                this.IndexOfEquals = null;
                this.IsLast = false;

                /// Читаем символ, и проверяем соответствие в скользящем окне
                for (this.Offset = 0; this.BinFileReader.BaseStream.Position < this.BinFileReader.BaseStream.Length; ++this.Offset)
                {
                    this.ReadedByte = this.BinFileReader.ReadByte();
                    /// Подготовка списка кондидитов на удаление для текущей итерации
                    this.IndexForRemove = new System.Collections.Generic.List<int>();

                    /// Создание буфера соответствий
                    if (this.IndexOfEquals == null)
                    {
                        this.IndexOfEquals = new System.Collections.Generic.List<Int32>();

                        this.IndexOfEquals = this.SlidingWindow.Select((x, i) => i).ToList();//(x == this.ReadedByte) ? i : -1).Where(x => x != -1).ToList();

                        /// Не найдено не одного соответствия
                        if (this.IndexOfEquals.Count < 1)
                        {
                            --this.BinFileReader.BaseStream.Position;
                            break;
                        }
                    }
                    else
                    {
                        foreach (Int32 x in IndexOfEquals)
                        {
                            /// Если не достигнут конец скользящего окна
                            if ((x + this.Offset) < SlidingWindow.Count)
                            {
                                /// Если элемент в окне не соответствует считанному с потока
                                if (this.SlidingWindow[x + this.Offset] != this.ReadedByte)
                                {
                                    /// Помечаем как кондидат на удаление
                                    this.IndexForRemove.Add(x);
                                }
                            }
                            else
                            {
                                /// Помечаем как кондидат на удаление
                                this.IndexForRemove.Add(x);
                            }
                        }

                        this.HelpDeleateOffset = 0;
                        foreach (Int32 x in IndexForRemove)
                        {
                            /// Если индекс последний
                            if (IndexOfEquals.Count() <= 1)
                            {
                                this.IsLast = true;
                                /// Т.к. Последнее считанное значение не найдено в сккользящем окне, оно  лишнее и его читать не следовало
                                --this.BinFileReader.BaseStream.Position;
                                break;
                            }
                            else
                            {
                                this.IndexOfEquals.Remove(x);// - HelpDeleateOffset);
                                ++this.HelpDeleateOffset;
                                continue;
                            }
                        }
                    }
                    if (IsLast)
                    {
                        break;
                    }
                }

                if ((this.Offset + 1) > Method.MinArchivingCount)
                {
                    //StreamPositionBuffer = this.BinFileWriter.BaseStream.Position;
                    this.BinFileReader.BaseStream.Position = this.StreamPositionBuffer;
                    this.InputByteBuffer = this.BinFileReader.ReadBytes(this.Offset + 1);
                    BinFileWriter.Write(this.IndexOfEquals[0].ToString());
                    BinFileWriter.Write(this.Offset.ToString());
                }
                else
                {
                    this.BinFileReader.BaseStream.Position = this.StreamPositionBuffer;
                    /// Чтение знаков из входного потока
                    this.InputByteBuffer = this.BinFileReader.ReadBytes(this.Offset + 1);
                    /// Запись в выходной
                    this.BinFileWriter.Write(InputByteBuffer);
                    /// Запись в скользящее окно
                    //this.SlidingWindow.RemoveRange(0, this.SlidingWindow.Count() - Method.BufferLengthByte);
                    this.SlidingWindow.AddRange(this.InputByteBuffer);
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
        private byte[] InputByteBuffer
        {
            get;
            set;
        }
        private Int64 StreamPositionBuffer
        {
            get;
            set;
        }
        private bool IsLast
        {
            get;
            set;
        }
        private Int32 Offset
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
        private UInt16 HelpDeleateOffset
        {
            get;
            set;
        }
        public const UInt16 BufferLengthByte = 1024;
        /// <summary>
        /// Значение равное 3 * (Тип записываемых в выходной поток меток в байтах)
        /// </summary>
        public const UInt16 MinArchivingCount = 6;
    }
}
