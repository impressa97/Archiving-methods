using System;
using System.Linq;
using System.IO.Compression;

namespace ar
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

            BinFileWriter.Write((UInt16)0);
            this.PreviousStructInStream = 0;

            this.SlidingWindow = new System.Collections.Generic.List<byte>();

            for (; this.BinFileReader.BaseStream.Position < this.BinFileReader.BaseStream.Length;)
            {
                /// Запомнили стартовую позицию указателя
                this.StreamPositionBuffer = this.BinFileReader.BaseStream.Position;
                this.IndexOfEquals = null;
                this.IsLast = false;
                this.Offset = 0;

                /// Читаем символ, и проверяем соответствие в скользящем окне
                for (; this.BinFileReader.BaseStream.Position < this.BinFileReader.BaseStream.Length;)
                {
                    this.ReadedByte = this.BinFileReader.ReadByte();
                    /// Подготовка списка кондидитов на удаление для текущей итерации
                    this.IndexForRemove = new System.Collections.Generic.List<int>();

                    /// Создание буфера соответствий
                    if (this.IndexOfEquals == null)
                    {
                        this.IndexOfEquals = new System.Collections.Generic.List<Int32>();

                        this.IndexOfEquals = this.SlidingWindow.Select((x, i) => (x == this.ReadedByte) ? i : -1).Where(x => x != -1).ToList();

                        /// Не найдено не одного соответствия
                        /// = Заход в первый раз.
                        if (this.IndexOfEquals.Count < 1)
                        {
                            ++this.Offset;
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
                                break;
                            }
                            else
                            {
                                this.IndexOfEquals.Remove(x);
                                ++this.HelpDeleateOffset;
                                continue;
                            }
                        }
                    }
                    if (IsLast)
                    {
                        break;
                    }
                    ++this.Offset;
                }

                /// Не пишим промежуточные(дополнительные отметки)
                if (this.Offset > (Method.MinArchivingCount - 1))
                {
                    this.BinFileReader.BaseStream.Position = this.StreamPositionBuffer;
                    this.InputByteBuffer = this.BinFileReader.ReadBytes(this.Offset);
                    BinFileWriter.Write((UInt16)0);

                    this.BinFileWriter.BaseStream.Position = this.PreviousStructInStream;
                    /// 4 = 2 * Sizeof(BufferLengthByte)
                    this.BinFileWriter.Write((UInt16)(this.BinFileWriter.BaseStream.Length - 4 - this.BinFileWriter.BaseStream.Position));
                    /// 2 = Sizeof(BufferLengthByte)
                    this.BinFileWriter.BaseStream.Position = this.BinFileWriter.BaseStream.Length;
                    this.PreviousStructInStream = this.BinFileWriter.BaseStream.Position - 2;

                    BinFileWriter.Write((UInt16)this.IndexOfEquals[0]);
                    BinFileWriter.Write((UInt16)this.Offset);                                                       ///?qwe_??qwsa

                    /*
                    this.IndexOfEquals.Add((UInt16)(this.BinFileWriter.BaseStream.Position - this.PreviousStructInStream));
                    this.StreamPositionBuffer = this.BinFileWriter.BaseStream.Position;
                    this.BinFileWriter.BaseStream.Position = this.PreviousStructInStream;
                    this.BinFileWriter.Write(this.IndexOfEquals.Last().ToString());
                    this.BinFileWriter.BaseStream.Position = this.StreamPositionBuffer + 1;
                    this.PreviousStructInStream = this.StreamPositionBuffer - 1;
                    */

                    /// Запись в скользящее окно
                    this.SlidingWindow.AddRange(InputByteBuffer);
                    if ((this.SlidingWindow.Count - 1) - Method.BufferLengthByte > 0)
                    {
                        this.SlidingWindow.RemoveRange(0, (this.SlidingWindow.Count - 1) - Method.BufferLengthByte);
                    }
                    this.IndexOfEquals.Clear();
                }
                else
                {
                    this.BinFileReader.BaseStream.Position = this.StreamPositionBuffer;
                    /// Чтение знаков из входного потока
                    this.InputByteBuffer = this.BinFileReader.ReadBytes(this.Offset);
                    /// Запись в выходной
                    this.BinFileWriter.Write(InputByteBuffer);
                    /// Запись в скользящее окно
                    this.SlidingWindow.AddRange(this.InputByteBuffer);
                    /// Чтобы не вылетал, когда не заполнен под завязку
                    if ((this.SlidingWindow.Count - 1) - Method.BufferLengthByte > 0)
                    {
                        this.SlidingWindow.RemoveRange(0, (this.SlidingWindow.Count - 1) - Method.BufferLengthByte);
                    }
                    this.IndexOfEquals.Clear();
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
        private Int64 PreviousStructInStream
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