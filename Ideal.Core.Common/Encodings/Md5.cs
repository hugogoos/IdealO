using System.Security.Cryptography;
using System.Text;

namespace Ideal.Core.Common.Encodings
{
    /// <summary>
    /// MD5加密
    /// </summary>
    public class Md5
    {
        /// <summary>
        /// netcore下的实现MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MD5Hash(string input)
        {
            using var md5 = MD5.Create();
            var result = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
            var strResult = BitConverter.ToString(result);
            return strResult.Replace("-", "");
        }
    }
}