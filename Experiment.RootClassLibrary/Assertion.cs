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

        public static int ReadBytes(this Stream inStream, Span<byte> buffer)
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
