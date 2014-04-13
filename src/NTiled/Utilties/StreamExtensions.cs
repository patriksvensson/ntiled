using System.IO;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace NTiled.Utilties
{
    public static class StreamExtensions
    {
        public static Stream GetDecompressor(this Stream input, string compression)
        {
            switch (compression.ToLower())
            {
                case "zlib":
                    return new InflaterInputStream(input);
                case "gzip":
                    return new GZipInputStream(input);
                default:
                    return input;
            }
        }
    }
}
