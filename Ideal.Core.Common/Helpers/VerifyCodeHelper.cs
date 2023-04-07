using SkiaSharp;

namespace Ideal.Core.Common.Helpers
{
    /// <summary>
    /// 验证码帮助类
    /// </summary>
    public class VerifyCodeHelper
    {
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static byte[] GetCaptcha(out string code)
        {
            var codeW = 80;
            var codeH = 30;
            var fontSize = 16;
            var chkCode = new char[4];

            char[] character = { '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'd', 'e', 'f', 'h', 'k', 'm', 'n', 'r', 'x', 'y', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
            var rnd = new Random();
            //生成验证码字符串
            for (var i = 0; i < 4; i++)
            {
                chkCode[i] = character[rnd.Next(character.Length)];
            }
            code = new string(chkCode);
            var bmp = new SKBitmap(codeW, codeH);
            using var canvas = new SKCanvas(bmp);
            //背景色
            canvas.DrawColor(SKColors.White);

            using (var sKPaint = new SKPaint())
            {
                sKPaint.TextSize = fontSize;//字体大小
                sKPaint.IsAntialias = true;//开启抗锯齿
                sKPaint.Typeface = SKTypeface.FromFamilyName("微软雅黑", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Italic);//字体
                var size = new SKRect();
                sKPaint.MeasureText(chkCode[0].ToString(), ref size);//计算文字宽度以及高度

                var temp = ((bmp.Width / 4) - size.Size.Width) / 2;
                var temp1 = bmp.Height - ((bmp.Height - size.Size.Height) / 2);
                var random = new Random();
                for (var i = 0; i < 4; i++)
                {
                    sKPaint.Color = new SKColor((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
                    canvas.DrawText(chkCode[i].ToString(), temp + (20 * i), temp1, sKPaint);//画文字
                }
                //干扰线
                for (var i = 0; i < 5; i++)
                {
                    sKPaint.Color = new SKColor((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
                    canvas.DrawLine(random.Next(0, 40), random.Next(1, 29), random.Next(41, 80), random.Next(1, 29), sKPaint);
                }
            }
            //页面展示图片
            using var img = SKImage.FromBitmap(bmp);
            using var p = img.Encode(SKEncodedImageFormat.Png, 100);
            return p.ToArray();
        }
    }
}
