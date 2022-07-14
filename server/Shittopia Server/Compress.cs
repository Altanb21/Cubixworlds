

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;


namespace Shittopia_Server
{
    internal static class Compress
    {
        public static string CompressString(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            MemoryStream memoryStream = new MemoryStream();
            using (GZipStream gzipStream = new GZipStream((Stream)memoryStream, CompressionMode.Compress, true))
                gzipStream.Write(bytes, 0, bytes.Length);
            memoryStream.Position = 0L;
            byte[] numArray1 = new byte[memoryStream.Length];
            memoryStream.Read(numArray1, 0, numArray1.Length);
            byte[] numArray2 = new byte[numArray1.Length + 4];
            Buffer.BlockCopy((Array)numArray1, 0, (Array)numArray2, 4, numArray1.Length);
            Buffer.BlockCopy((Array)BitConverter.GetBytes(bytes.Length), 0, (Array)numArray2, 0, 4);
            return Convert.ToBase64String(numArray2);
        }

        public static string DecompressString(string compressedText)
        {
            byte[] buffer = Convert.FromBase64String(compressedText);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                int int32 = BitConverter.ToInt32(buffer, 0);
                memoryStream.Write(buffer, 4, buffer.Length - 4);
                byte[] numArray = new byte[int32];
                memoryStream.Position = 0L;
                using (GZipStream gzipStream = new GZipStream((Stream)memoryStream, CompressionMode.Decompress))
                    gzipStream.Read(numArray, 0, numArray.Length);
                return Encoding.UTF8.GetString(numArray);
            }
        }

        public static byte[] CompressBytes(byte[] data)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (DeflateStream deflateStream = new DeflateStream((Stream)memoryStream, CompressionLevel.Optimal))
                deflateStream.Write(data, 0, data.Length);
            return memoryStream.ToArray();
        }

        public static byte[] DecompressBytes(byte[] data)
        {
            MemoryStream memoryStream = new MemoryStream(data);
            MemoryStream destination = new MemoryStream();
            using (DeflateStream deflateStream = new DeflateStream((Stream)memoryStream, CompressionMode.Decompress))
                deflateStream.CopyTo((Stream)destination);
            return destination.ToArray();
        }

        public static Image GetCompressedBitmap(Bitmap bmp, long quality)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                EncoderParameter encoderParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                ImageCodecInfo encoder = ((IEnumerable<ImageCodecInfo>)ImageCodecInfo.GetImageEncoders()).FirstOrDefault<ImageCodecInfo>((Func<ImageCodecInfo, bool>)(o => o.FormatID == ImageFormat.Jpeg.Guid));
                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = encoderParameter;
                bmp.Save((Stream)memoryStream, encoder, encoderParams);
                return Image.FromStream((Stream)memoryStream);
            }
        }
    }
}
