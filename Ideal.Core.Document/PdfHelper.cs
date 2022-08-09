//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.tool.xml;
//using System;
//using System.IO;
//using System.Text;

//namespace Ideal.Core.Document
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public class PdfHelper
//    {
//        /// <summary>
//        /// 根据html字符串生成pdf
//        /// </summary>
//        /// <param name="filename">pdf完全名称</param>
//        /// <param name="html">html字符串</param>
//        public static void Write(string filename, string html)
//        {
//            if (File.Exists(filename))
//            {
//                File.Delete(filename);
//            }

//            var bytes = WriteBytes(html);
//            using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
//            {
//                fs.Write(bytes, 0, bytes.Length);
//            }
//        }

//        /// <summary>
//        /// 根据html字符串生成pdf字节数组
//        /// </summary>
//        /// <param name="html">html字符串</param>
//        /// <returns>pdf字节数组</returns>
//        public static byte[] WriteBytes(string html)
//        {
//            var data = Encoding.UTF8.GetBytes(html); // 字串转成byte[]  

//            using (var inputStream = new MemoryStream(data))
//            using (var outputStream = new MemoryStream())   // 要把PDF写到哪个串流
//            using (var doc = new iTextSharp.text.Document()) // 要写PDF的文件，建构子没填的话预设直式A4
//            using (var writer = PdfWriter.GetInstance(doc, outputStream))
//            {
//                var pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc.PageSize.Height, 1f);     // 指定文件预设开档时的缩放为100%   

//                doc.Open();
//                XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, inputStream, null, Encoding.UTF8, new UnicodeFontFactory());  // 使用XMLWorkerHelper把Html parse到PDF档里
//                var action = PdfAction.GotoLocalPage(1, pdfDest, writer);   // 将pdfDest设定的资料写到PDF档 
//                writer.SetOpenAction(action);
//                doc.Close();

//                var output = outputStream.ToArray();
//                return output;
//            }
//        }

//        /// <summary>
//        /// 根据html字符串生成pdf内存流
//        /// </summary>
//        /// <param name="html">html字符串</param>
//        /// <returns>pdf内存流</returns>
//        public static MemoryStream WriteMemoryStream(string html)
//        {
//            var bytes = WriteBytes(html);
//            return new MemoryStream(bytes);
//        }
//    }
//    public class UnicodeFontFactory : FontFactoryImp
//    {
//        private static readonly string arialFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
//            "arialuni.ttf");//arial unicode MS是完整的unicode字型。
//        private static readonly string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
//          "simkai.ttf");//標楷體


//        public override Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color,
//            bool cached)
//        {
//            //可用Arial或標楷體，自己選一個
//            var baseFont = BaseFont.CreateFont(path, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
//            return new Font(baseFont, size, style, color);
//        }
//    }
//}
