using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace Ideal.Core.Common.Extensions
{
    /// <summary>
    /// 编码解码相关扩展方法
    /// </summary>
    public static class CodecExtension
    {
        /// <summary>
        /// 压缩字节数组
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static byte[] Compress(this byte[] source)
        {
            var output = new MemoryStream();
            using (var compressedStream = new DeflaterOutputStream(output))
            {
                compressedStream.Write(source, 0, source.Length);
            }

            return output.ToArray();
        }

        /// <summary>
        /// 解压字节数组
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static byte[] Decompress(this byte[] source)
        {
            using var stream = new InflaterInputStream(new MemoryStream(source));
            using var output = new MemoryStream();
            StreamUtils.Copy(stream, output, new byte[4096]);
            return output.ToArray();
        }
    }
}
