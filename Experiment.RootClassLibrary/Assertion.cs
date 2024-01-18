using System;
using System.IO;

namespace Experiment.RootClassLibrary
{
    public static class Assertion
    {
        public static void ReadBuffer(Stream inStream, Span<byte> buffer)
        {
            System.Diagnostics.Debug.Assert(inStream.ReadBytes(buffer) == buffer.Length, "inStream.ReadBytes(buffer) == buffer.Length");
        }

        /// <summary>
        /// 指定されたバッファが満たされるまでストリームから読み込みを行います。
        /// </summary>
        /// <param name="inStream">
        /// 入力ストリームである <see cref="Stream"/> オブジェクトです。
        /// </param>
        /// <param name="buffer">
        /// ストリームから読み込んだデータを格納するためのバッファです。
        /// </param>
        /// <returns>
        /// 実際に読み込むことが出来たデータの長さです。
        /// </returns>
        /// <remarks>
        /// 途中で入力ストリームの終端に達した場合を除き、このメソッドは必ず <paramref name="buffer"/> の長さのデータを入力ストリームから読み込みます。
        /// </remarks>
        private static int ReadBytes(this Stream inStream, Span<byte> buffer)
        {
            var span = buffer;
            while (!span.IsEmpty)
            {
                var length = inStream.Read(span);
                if (length <= 0)
                    break;
                span = span[length..];
            }

            return buffer.Length - span.Length;
        }
    }
}
