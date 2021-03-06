﻿using System.IO;

namespace CSharpUtils.Streams
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class FilterStream : Stream
    {
        /// <summary>
        /// 
        /// </summary>
        public Stream ParentStream;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentStream"></param>
        protected FilterStream(Stream parentStream)
        {
            ParentStream = parentStream;
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool CanRead => ParentStream.CanRead;

        /// <summary>
        /// 
        /// </summary>
        public override bool CanSeek => ParentStream.CanSeek;

        /// <summary>
        /// 
        /// </summary>
        public override bool CanWrite => ParentStream.CanWrite;

        /// <summary>
        /// 
        /// </summary>
        public override void Flush()
        {
            ParentStream.Flush();
        }

        /// <summary>
        /// 
        /// </summary>
        public override long Length => ParentStream.Length;

        /// <summary>
        /// 
        /// </summary>
        public override long Position
        {
            get => ParentStream.Position;
            set => ParentStream.Position = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            return ParentStream.Read(buffer, offset, count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            return ParentStream.Seek(offset, origin);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            ParentStream.SetLength(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            ParentStream.Write(buffer, offset, count);
        }
    }
}